using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    public void Awake()
    {
        if (instance != null) 
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        //Debug.Log("Persistent path: " + Application.persistentDataPath); /Users/kevinzhu/Library/Application Support/DefaultCompany/RPG
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        IEnumerable<ISaveManager> saveManagers = monoBehaviours.OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }


}
