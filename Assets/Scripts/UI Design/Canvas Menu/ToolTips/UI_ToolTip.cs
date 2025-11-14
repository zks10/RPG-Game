using UnityEngine;
using UnityEngine.UI;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] protected Vector2 offset = new Vector2(15f, -15f);
    [SerializeField] protected float followSpeed = 20f;
    protected RectTransform rectTransform;
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    protected virtual void Update()
    {
        // if (gameObject.activeSelf)
        //     UpdatePosition();
    }

    protected virtual void UpdatePosition(bool instant = false)
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 tooltipSize = rectTransform.sizeDelta * canvas.scaleFactor;
        Vector2 targetPos = mousePos + offset;

        float screenW = Screen.width;
        float screenH = Screen.height;

        if (mousePos.x + tooltipSize.x + offset.x > screenW)
        {
            targetPos.x = mousePos.x - tooltipSize.x - offset.x;
        }

        if (mousePos.y - tooltipSize.y + offset.y < 0)
        {
            targetPos.y = mousePos.y + tooltipSize.y - offset.y;
        }


        rectTransform.position = targetPos;
    }


    public CanvasGroup CanvasGroup => canvasGroup;
    
    public virtual void ShowInstantlyAtMouse()
    {
        
    }

    public virtual void HideToolTips() { }

    public virtual void ShowToolTips() { } 
    public virtual void ShowToolTips(string description) { }
    public virtual void ShowToolTips(ItemData_Equipment item) { }
    public virtual void ShowToolTips(string skillDesc, string skillName, int skillPrice) { }

}
