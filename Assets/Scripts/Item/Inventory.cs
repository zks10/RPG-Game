using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemData> startingItems;

    public List<InventoryItem> inventoryItem;

    public List<InventoryItem> stashItem;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipmentItem;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    public List<InventoryItem> edibleItem;
    public Dictionary<ItemData_Edible, InventoryItem> edibleDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform edibleSlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlotEquipment[] equipmentSlot;
    private UI_ItemSlot[] edibleSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedTrinket = Mathf.NegativeInfinity;
    private float lastTimeUsedArmor = Mathf.NegativeInfinity;
    private float trinketCooldown;
    private float armorCooldown;
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
        
        stashItem = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentItem = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        edibleItem = new List<InventoryItem>();
        edibleDictionary = new Dictionary<ItemData_Edible, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_ItemSlotEquipment>();
        edibleSlot = edibleSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.slotType == newEquipment.slotType)
                oldEquipment = item.Key;

        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipmentItem.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);
        UpdateSlotUI();
    }

    public void UnEquipItem(ItemData_Equipment oldEquipment)
    {

        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipmentItem.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();
        }

    }
    
    private void UpdateSlotUI()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.slotType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);

            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUp();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUp();
        }
        for (int i = 0; i < edibleSlot.Length; i++)
        {
            edibleSlot[i].CleanUp();
        }
        // for (int i = 0; i < equipmentSlot.Length; i++)
        // {
        //     equipmentSlot[i].CleanUp();
        // }

        for (int i = 0; i < inventoryItem.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryItem[i]);
        }
        for (int i = 0; i < stashItem.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stashItem[i]);
        }
        for (int i = 0; i < edibleItem.Count; i++)
        {
            edibleSlot[i].UpdateSlot(edibleItem[i]);
        }
        // for (int i = 0; i < equipmentItem.Count; i++)
        // {
        //     equipmentSlot[i].UpdateSlot(equipmentItem[i]);
        // }
    }
    
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);
        else if (_item.itemType == ItemType.Edible)
            AddToEdible(_item);
        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        inventoryItem.Add(newItem);
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItem.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToEdible(ItemData _item)
    {
        if (!(_item is ItemData_Edible edibleData))
            return; 

        if (edibleDictionary.TryGetValue(edibleData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(edibleData);
            edibleItem.Add(newItem);
            edibleDictionary.Add(edibleData, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        // --- INVENTORY ---
        for (int i = 0; i < inventoryItem.Count; i++)
        {
            if (inventoryItem[i].data == _item)
            {
                inventoryItem.RemoveAt(i);
                UpdateSlotUI();
                return;
            }
        }

        // --- STASH ---
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashItem.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        // --- EDIBLE (this is the important fix) ---
        if (_item is ItemData_Edible edibleData)
        {
            if (edibleDictionary.TryGetValue(edibleData, out InventoryItem edibleValue))
            {
                if (edibleValue.stackSize <= 1)
                {
                    edibleItem.Remove(edibleValue);
                    edibleDictionary.Remove(edibleData);
                }
                else
                {
                    edibleValue.RemoveStack();
                }
            }
        }

        UpdateSlotUI();
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials (1)");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("Not enough materials (2)");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipmentItem;
    public List<InventoryItem> GetStashList() => stashItem;
    public List<InventoryItem> GetEdibleList() => edibleItem;
    public ItemData_Equipment GetEquipementByType(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.slotType == _type)
                equipedItem = item.Key;

        }
        return equipedItem;

    }

    public void UseTrinket()
    {
        ItemData_Equipment currentTrinket = GetEquipementByType(EquipmentType.Trinket);

        if (currentTrinket == null)
            return;

        bool canUseTrinket = Time.time > lastTimeUsedTrinket + trinketCooldown;

        if (canUseTrinket)
        {
            trinketCooldown = currentTrinket.itemCooldown;
            currentTrinket.ItemEffect(null);
            lastTimeUsedTrinket = Time.time;
        }
        else
            Debug.Log("Trinket is in cooldown.");
    }

    public void ConsumeEdibles(ItemData _item)
    {
        if (!(_item is ItemData_Edible edibleData))
            return;

        ItemData_Edible newEdible = _item as ItemData_Edible;
        newEdible.ItemEffect(null);
        RemoveItem(_item);
        UpdateSlotUI();
    }
    
    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipementByType(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("Armor is in cooldown.");
        return false;
    }

}
