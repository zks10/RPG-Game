using UnityEngine;
using UnityEngine.EventSystems; 

public class UI_ItemSlotEquipment : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot : " + slotType.ToString();
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnEquipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanUp();
    }
}
