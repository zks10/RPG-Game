using UnityEngine;
using TMPro;



public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null)
            return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.slotType.ToString();
        itemDescription.text = item.GetDescription();

        gameObject.SetActive(true);

    }

    public void HideToolTip() => gameObject.SetActive(false);
}
