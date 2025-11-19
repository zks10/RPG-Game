 using UnityEngine;
 using System.Collections.Generic;

[System.Serializable]
public class GameData 
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;
    public SerializableDictionary<string, bool> checkpoints;
    public string lastCheckpointId;


    public GameData()
    {
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        checkpoints = new SerializableDictionary<string, bool>();
        lastCheckpointId = string.Empty;

    }
}
