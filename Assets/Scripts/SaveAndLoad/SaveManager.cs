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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
                //Debug.Log("Persistent path: " + Application.persistentDataPath); /Users/kevinzhu/Library/Application Support/DefaultCompany/RPG

        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    private void Start()
    {
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

    // private List<ISaveManager> FindAllSaveManagers ()
    // {
    //     IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
    //     return new List<ISaveManager>(saveManagers);
    // }
    private List<ISaveManager> FindAllSaveManagers()
    {
        // The second parameter allows you to include inactive objects
        MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        IEnumerable<ISaveManager> saveManagers = monoBehaviours.OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

}
