using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour, ISaveManager
{   
    public static GameManager instance;
    private Transform player;
    [SerializeField] private Checkpoint[] checkpoints;
    public string lastCheckpointId;

    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    [SerializeField] private float lostPosX;
    [SerializeField] private float lostPosY;
    public int lostCurrencyAmount;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        
    }
    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));

    }

    public void SaveData(ref GameData _data)
    {
        //_data.lastCheckpointId = FindClosestCheckpoint().id; use closest (not like this way)
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostPosCurrencyX = player.position.x;
        _data.lostPosCurrencyY = player.position.y - 0.505f;

        if (lastCheckpointId != null)
            _data.lastCheckpointId = lastCheckpointId;

        _data.checkpoints.Clear();
        foreach(Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activeStatus);
        }

    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();
                    
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostPosX = _data.lostPosCurrencyX;
        lostPosY = _data.lostPosCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostPosX, lostPosY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }
    public IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);
        LoadAtLastCheckpoint(_data);
        LoadLostCurrency(_data);

    }
    public void LoadAtLastCheckpoint(GameData _data)
    {
        if (_data.lastCheckpointId == null)
            return;

        lastCheckpointId = _data.lastCheckpointId;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (lastCheckpointId == checkpoint.id)
            {
                player.position = checkpoint.transform.position;
            }
        }
    }
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        Vector2 playerPos = PlayerManager.instance.player.transform.position;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (!checkpoint.activeStatus)
                continue; // skip inactive checkpoints

            float distanceToCheckpoint = Vector2.Distance(playerPos, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
    public void SetLastCheckpoint(string _checkpointId)
    {
        lastCheckpointId = _checkpointId;
    }


}
