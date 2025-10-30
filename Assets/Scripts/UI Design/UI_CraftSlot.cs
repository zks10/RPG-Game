using UnityEngine;
using UnityEngine.EventSystems; 

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);

        // if (Inventory.instance.canCraft(craftData, craftData.craftingMaterials))
        // {
        //     play sound

        // }
    }
}
