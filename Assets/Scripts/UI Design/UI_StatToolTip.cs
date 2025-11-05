using UnityEngine;
using TMPro;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Vector2 offset = new Vector2(20f, -10f);
    [SerializeField] private float followSpeed = 20f;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        gameObject.SetActive(false);
    }

    public void ShowStatToolTip(string _text)
    {
        description.text = _text;
        gameObject.SetActive(true);
        UpdatePosition(true);
    }

    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeSelf)
            UpdatePosition();
    }

    private void UpdatePosition(bool instant = false)
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 tooltipSize = rectTransform.sizeDelta * canvas.scaleFactor;
        Vector2 targetPos = mousePos + offset;

        float screenW = Screen.width;
        float screenH = Screen.height;

        // --- Flip horizontally if too close to right edge ---
        if (mousePos.x + tooltipSize.x + offset.x > screenW)
        {
            targetPos.x = mousePos.x - tooltipSize.x - offset.x;
        }

        // --- Flip vertically if too close to top edge ---
        if (mousePos.y - tooltipSize.y + offset.y < 0)
        {
            targetPos.y = mousePos.y + tooltipSize.y - offset.y;
        }

        // --- Smooth movement ---
        Vector2 newPos = instant
            ? targetPos
            : Vector2.Lerp(rectTransform.position, targetPos, Time.deltaTime * followSpeed);

        rectTransform.position = newPos;
    }
}
