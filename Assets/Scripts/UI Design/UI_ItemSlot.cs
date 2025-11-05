using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    private UI ui;
    public InventoryItem item;

    public void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;
        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUp()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (item.data == null)
            return;

        if (Inventory.instance == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
            Inventory.instance.RemoveItem(item.data);
        else if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
        else if (item.data.itemType == ItemType.Edible)
            Inventory.instance.ConsumeEdibles(item.data);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;
         ui.itemToolTip.HideToolTip();
    }
    
}
