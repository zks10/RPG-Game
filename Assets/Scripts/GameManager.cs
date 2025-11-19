using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, ISaveManager
{   
    public static GameManager instance;
    [SerializeField] private Checkpoint[] checkpoints;
    public string lastCheckpointId;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();
                    
            }
        }
        lastCheckpointId = _data.lastCheckpointId;
        Invoke("PlacePlayerAtLastCheckpoint", .1f);

    }

    public void PlacePlayerAtLastCheckpoint()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (lastCheckpointId == checkpoint.id)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }
    public void SaveData(ref GameData _data)
    {
        //_data.lastCheckpointId = FindClosestCheckpoint().id; use closest (not like this way)
        _data.lastCheckpointId = lastCheckpointId;
        _data.checkpoints.Clear();
        foreach(Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activeStatus);
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
