using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Inventory : MonoBehaviour, ISaveManager
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
    [SerializeField] private Transform stashCraftSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform edibleSlotParent;
    [SerializeField] private Transform statSlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlot[] stashCraftSlot;
    private UI_ItemSlotEquipment[] equipmentSlot;
    private UI_ItemSlot[] edibleSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedTrinket = Mathf.NegativeInfinity;
    private float lastTimeUsedArmor = Mathf.NegativeInfinity;
    public float trinketCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems; 
    public List<ItemData_Equipment> loadedEquipment;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        
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
        stashCraftSlot = stashCraftSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_ItemSlotEquipment>();
        edibleSlot = edibleSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach(ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem loadedItem in loadedItems)
            {
                switch (loadedItem.data.itemType)
                {
                    case ItemType.Equipment:
                        AddItem(loadedItem.data); // Each equipment is always one per slot
                        break;
                    case ItemType.Material:
                    case ItemType.Edible:
                        // Use stack size for stackable items
                        for (int i = 0; i < loadedItem.stackSize; i++)
                            AddItem(loadedItem.data);
                        break;
                }
            }
            return;
        }

        foreach (ItemData item in startingItems)
        {
            if (item != null)
                AddItem(item);
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
        AudioManager.instance.PlaySFX(11, null);
        RemoveItem(_item);
        UpdateSlotUI();
    }

    public void UnEquipItem(ItemData_Equipment oldEquipment)
    {

        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipmentItem.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            AudioManager.instance.PlaySFX(11, null);
            oldEquipment.RemoveModifiers();
        }

    }
    
    private void UpdateSlotUI()
    {
        // --- Equipment Slots ---
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.slotType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        // --- Cleanup all slots first ---
        for (int i = 0; i < inventoryItemSlot.Length; i++)
            inventoryItemSlot[i].CleanUp();

        for (int i = 0; i < stashItemSlot.Length; i++)
            stashItemSlot[i].CleanUp();

        for (int i = 0; i < stashCraftSlot.Length; i++)
            stashCraftSlot[i].CleanUp();

        for (int i = 0; i < edibleSlot.Length; i++)
            edibleSlot[i].CleanUp();

        UpdateStatUI();


        // --- Inventory Items ---
        for (int i = 0; i < inventoryItem.Count && i < inventoryItemSlot.Length; i++)
            inventoryItemSlot[i].UpdateSlot(inventoryItem[i]);


        // --- Stash Items (shared between both stash UIs) ---
        int stashCount = stashItem.Count;

        for (int i = 0; i < stashCount && i < stashItemSlot.Length; i++)
            stashItemSlot[i].UpdateSlot(stashItem[i]);

        for (int i = 0; i < stashCount && i < stashCraftSlot.Length; i++)
            stashCraftSlot[i].UpdateSlot(stashItem[i]);


        // --- Edible Items ---
        for (int i = 0; i < edibleItem.Count && i < edibleSlot.Length; i++)
            edibleSlot[i].UpdateSlot(edibleItem[i]);
    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
            statSlot[i].UpdateStatValueUI();
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddEquipItem())
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

    public bool CanAddEquipItem()
    {
        if (inventoryItem.Count >= inventoryItemSlot.Length)
            return false;
        return true;
    }
    public bool CanCraftEquipment(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        foreach (var requiredItem in _requiredMaterials)
        {
            if (stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requiredItem.stackSize) 
                {
                    Debug.Log("Not enough materials: " + requiredItem.data.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("Materials not found in stash: " + requiredItem.data.name);
                return false;
            }
        }
        foreach (var requiredMaterial in _requiredMaterials)
        {
            for (int i = 0; i < requiredMaterial.stackSize; i++)
            {
                RemoveItem(requiredMaterial.data);
            }
        }

        AddItem(_itemToCraft);

        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipmentItem;
    public List<InventoryItem> GetStashList() => stashItem;
    public List<InventoryItem> GetEdibleList() => edibleItem;
    
    // public ItemData_Equipment GetEquipmentByType(EquipmentType _type)
    // {
    //     ItemData_Equipment equipedItem = null;

    //     foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
    //     {
    //         if (item.Key.slotType == _type)
    //             equipedItem = item.Key;
 
    //     }
    //     return equipedItem;

    // }
    public ItemData_Equipment GetEquipmentByType(EquipmentType _type)
    {
        if (equipmentDictionary == null)
            return null;

        foreach (var item in equipmentDictionary)
            if (item.Key.slotType == _type)
                return item.Key;

        return null;
    }


    public void UseTrinket()
    {
        ItemData_Equipment currentTrinket = GetEquipmentByType(EquipmentType.Trinket);

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
            PlayerManager.instance.player.fx.CreatePopUpText("Cooldown");
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
        ItemData_Equipment currentArmor = GetEquipmentByType(EquipmentType.Armor);

        if (Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }
        Debug.Log("Armor is in cooldown.");
        return false;
    }
    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    // Check the type of item
                    if (item.itemType == ItemType.Equipment)
                    {
                        // Non-stackable, add one item per saved count (should normally be 1)
                        for (int i = 0; i < pair.Value; i++)
                        {
                            InventoryItem loadedItem = new InventoryItem(item);
                            loadedItems.Add(loadedItem);
                        }
                    }
                    else
                    {
                        // Stackable (Material or Edible), add one InventoryItem with stackSize = pair.Value
                        InventoryItem loadedItem = new InventoryItem(item);
                        loadedItem.stackSize = pair.Value;
                        loadedItems.Add(loadedItem);
                    }
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        Dictionary<string, int> counts = new Dictionary<string, int>();

        foreach (InventoryItem item in inventoryItem)
        {
            string id = item.data.itemId;

            if (!counts.ContainsKey(id))
                counts[id] = 0;

            counts[id]++;
        }

        foreach (var pair in counts)
            _data.inventory.Add(pair.Key, pair.Value);
        
        
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        } 
        foreach (KeyValuePair<ItemData_Edible, InventoryItem> pair in edibleDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        } 
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        } 
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item database")]
    private void FillUpItemDatabase() => itemDatabase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Items Data/Items"});

        foreach(string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDatabase.Add(itemData);
        }
        return itemDatabase;
    }
#endif

}
