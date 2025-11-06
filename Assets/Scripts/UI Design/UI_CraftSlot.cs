using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftSlot : UI_ItemSlot, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Visual Feedback")]
    public Image backgroundImage; // assign your slot background (not the item icon)
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.9f, 0.9f, 1f, 1f); // soft bluish highlight
    public Color clickColor = new Color(0.8f, 0.8f, 1f, 1f); // slightly darker
    public float hoverScale = 1.05f;

    private Vector3 originalScale;
    private bool isHovered = false;

    protected override void Start()
    {
        base.Start();
        originalScale = transform.localScale;

        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>(); // auto-assign if possible

        ResetVisuals();
    }

    public void SetUpCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
            return;

        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData); // just in case UI_ItemSlot has logic
        ui.craftWindow.SetUpCraftWindow(item.data as ItemData_Equipment);

        // Optional: brief click visual feedback
        SetColor(clickColor);
        CancelInvoke(nameof(ResetVisuals));
        Invoke(nameof(ResetVisuals), 0.15f);

        // You can also play a sound here
        // AudioManager.Play("CraftSlotClick");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        SetColor(hoverColor);
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        ResetVisuals();
    }

    private void ResetVisuals()
    {
        SetColor(normalColor);
        transform.localScale = originalScale;
    }

    private void SetColor(Color color)
    {
        if (backgroundImage != null)
            backgroundImage.color = color;
    }
}
