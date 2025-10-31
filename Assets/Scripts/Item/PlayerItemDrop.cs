using UnityEngine;
using System.Collections.Generic;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    public override void GenerateDrops()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> stashToLose = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnEquipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                DropItem(item.data);
                stashToLose.Add(item);
            }
        }
        for (int i = 0; i < stashToLose.Count; i++)
        {
            inventory.RemoveItem(stashToLose[i].data);
        }
        
        
    }
}
