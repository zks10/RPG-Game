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
    public float lostPosCurrencyX;
    public float lostPosCurrencyY;
    public int lostCurrencyAmount;
    public SerializableDictionary<string, float> volumeSettings;


    public GameData()
    {
        this.lostPosCurrencyX = 0;
        this.lostPosCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        checkpoints = new SerializableDictionary<string, bool>();
        lastCheckpointId = string.Empty;
        volumeSettings = new SerializableDictionary<string, float>();

    }
}
