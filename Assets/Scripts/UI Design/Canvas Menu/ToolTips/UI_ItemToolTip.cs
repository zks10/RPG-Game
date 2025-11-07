using UnityEngine;
using TMPro;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowItemToolTip(ItemData_Equipment item)
    {
        if (item == null)
            return;
        itemNameText.text = item.itemName;
        itemTypeText.text = item.slotType.ToString();
        itemDescription.text = item.GetDescription();

        gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        // Now update the position with the fresh size
        UpdatePosition(true);

        // Finally make tooltip visible
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

    }

    public void HideItemToolTip()
    {
        itemTypeText.text = "";
        gameObject.SetActive(false);
    }
    
    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdatePosition(bool instant = false)
    {
        base.UpdatePosition(instant);
    }

    public override void ShowInstantlyAtMouse()
    {
        base.ShowInstantlyAtMouse();
    }
}
