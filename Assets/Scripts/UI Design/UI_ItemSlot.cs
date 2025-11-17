using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using UnityEngine.UI; 
public class UI_ItemSlot : UI_Slots, IPointerDownHandler
{
    public InventoryItem item;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    protected override void Start()
    {
        base.Start();
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

        // Stop any tooltip coroutines
        if (hoverDelayCoroutine != null)
        {
            StopCoroutine(hoverDelayCoroutine);
            hoverDelayCoroutine = null;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (ui != null && ui.itemToolTip != null)
            ui.itemToolTip.HideItemToolTip();

        isTooltipVisible = false;
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

        ui.itemToolTip.HideItemToolTip();
    }

    public override void ShowToolTip()
    {
        base.ShowToolTip();
        if (item == null) 
        {
            tooltip = null;
            return;
        }
        ui.itemToolTip.ShowItemToolTip(item.data as ItemData_Equipment);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public override void AssignToolTip()
    {
        base.AssignToolTip();
        tooltip = ui.itemToolTip;
    }

    public override void HideToolTip()
    {
        base.HideToolTip();
        tooltip.HideToolTips();
    }

    public override void ToolTipShowToolTip()
    {
        base.ToolTipShowToolTip();
        if (item == null) 
        {
            tooltip = null;
            return;
        }
        tooltip.ShowToolTips(item.data as ItemData_Equipment);
    }

    public override IEnumerator FadeAndSlideTooltipCoroutine(float targetAlpha)
    {
        yield return base.FadeAndSlideTooltipCoroutine(targetAlpha);
    }

    public override void StopOtherFadeIfRunning()
    {
        base.StopOtherFadeIfRunning();
    }
    
}
