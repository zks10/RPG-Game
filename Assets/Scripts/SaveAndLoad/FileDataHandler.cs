using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writter = new StreamWriter(stream))
                {
                    writter.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file:" + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Save file not found. Creating new data.");
        }

        return loadData;
    }





}
