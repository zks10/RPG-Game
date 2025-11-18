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

        if (item == null || item.data == null)
            return;
        
        ItemData_Equipment equipmentData = item.data as ItemData_Equipment;
        if (equipmentData == null)
            return;

        Inventory.instance.UnEquipItem(equipmentData);
        Inventory.instance.AddItem(equipmentData);

        ui.itemToolTip.HideItemToolTip();
        CleanUp();
    }

}
