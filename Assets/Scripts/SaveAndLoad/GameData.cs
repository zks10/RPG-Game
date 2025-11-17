 using UnityEngine;
 using System.Collections.Generic;

[System.Serializable]
public class GameData 
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
    }
}
