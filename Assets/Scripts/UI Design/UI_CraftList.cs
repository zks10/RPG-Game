using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;


    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetUpCraftList();
        SetUpDefaultCraftWindow();
    }

    public void SetUpCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
            Destroy(craftSlotParent.GetChild(i).gameObject);

  

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);

        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }

    public void SetUpDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetUpCraftWindow(craftEquipment[0]); 
    }
     
}
