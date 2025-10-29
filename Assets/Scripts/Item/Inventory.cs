using UnityEngine;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inventoryItem;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        inventoryItem = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
    }
    public void AddItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItem.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItem.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && inventoryItem.Count > 0)
        {
            ItemData newItem = inventoryItem[inventoryItem.Count - 1].data;
            RemoveItem(newItem);
        }
    }


}
