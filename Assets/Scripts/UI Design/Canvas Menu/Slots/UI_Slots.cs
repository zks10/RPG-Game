using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UI_Slots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;
    protected bool isHovering = false;
    protected bool isTooltipVisible = false;
    protected Coroutine fadeCoroutine;
    protected Coroutine hoverDelayCoroutine;

    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float slideDistance = 10f;
    [SerializeField] private float hoverDelay = 0.2f;

    protected UI_SkillToolTip tooltip;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    protected virtual void Update()
    {
        bool controlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (isHovering && controlHeld)
        {
            if (!isTooltipVisible && hoverDelayCoroutine == null)
                hoverDelayCoroutine = StartCoroutine(HoverDelayShow());
        }
        else
        {
            if (hoverDelayCoroutine != null)
            {
                StopCoroutine(hoverDelayCoroutine);
                hoverDelayCoroutine = null;
            }

            if (isTooltipVisible)
            {
                StartFadeAndSlideTooltip(0f);
                isTooltipVisible = false;
            } 
        }
    }

    public virtual IEnumerator HoverDelayShow()
    {
        yield return new WaitForSeconds(hoverDelay);

        bool controlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (isHovering && controlHeld)
        {
            StopOtherFadeIfRunning();
            ShowToolTip();
            StartFadeAndSlideTooltip(1f);
            isTooltipVisible = true;
        }

        hoverDelayCoroutine = null;
    }

    public virtual void ShowToolTip() { }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        StopOtherFadeIfRunning();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (hoverDelayCoroutine != null)
        {
            StopCoroutine(hoverDelayCoroutine);
            hoverDelayCoroutine = null;
        }

        if (isTooltipVisible)
        {
            StartFadeAndSlideTooltip(0f);
            isTooltipVisible = false;
        }
    }

    #region Fade + Slide Animation

    public virtual void StartFadeAndSlideTooltip(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndSlideTooltipCoroutine(targetAlpha));
    }

    public virtual void AssignToolTip() { }

    public virtual void HideToolTip() 
    {
        
    }

    public virtual void ToolTipShowToolTip() { }

    public virtual IEnumerator FadeAndSlideTooltipCoroutine(float targetAlpha)
    {
        AssignToolTip();
        CanvasGroup canvasGroup = tooltip.GetComponent<CanvasGroup>();
        RectTransform rectTransform = tooltip.GetComponent<RectTransform>();

        if (canvasGroup == null || rectTransform == null)
        {
            if (targetAlpha == 0f)
                HideToolTip();
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + (targetAlpha > 0 ? slideDistance : -slideDistance), startPos.z);

        if (targetAlpha > 0f)
        {
            ToolTipShowToolTip();
            canvasGroup.blocksRaycasts = false;
        }

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
            HideToolTip();
    }

    public virtual void StopOtherFadeIfRunning()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        AssignToolTip();
        CanvasGroup cg = tooltip.GetComponent<CanvasGroup>();
        if (cg != null && cg.alpha < 1f)
            cg.alpha = 1f;
    }

    #endregion
}
