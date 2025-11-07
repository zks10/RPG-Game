using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; 
using System.Collections;


public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui; 
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;    
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField] private string statDescription;

    private bool isHovering = false;
    private bool isTooltipVisible = false;
    private Coroutine fadeCoroutine;
    private Coroutine hoverDelayCoroutine;

    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float slideDistance = 10f;
    [SerializeField] private float hoverDelay = 0.2f;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
            statNameText.text = statName;
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetCalculatedStatValue(statType).ToString();
        }
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        UpdateStatValueUI();
    }
    private void Update()
    {
        bool controlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (isHovering && controlHeld)
        {
            // Start hover delay if not already visible
            if (!isTooltipVisible && hoverDelayCoroutine == null)
                hoverDelayCoroutine = StartCoroutine(HoverDelayShow());
        }
        else
        {
            // Cancel hover delay if user stops hovering or releases control
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

    private IEnumerator HoverDelayShow()
    {
        yield return new WaitForSeconds(hoverDelay);

        bool controlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        if (isHovering && controlHeld)
        {
            // 🧩 FIX: Immediately stop any previous fade-out to avoid conflicts
            StopOtherFadeIfRunning();

            ui.statToolTip.ShowStatToolTip(statDescription);
            StartFadeAndSlideTooltip(1f);
            isTooltipVisible = true;
        }

        hoverDelayCoroutine = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;

        // 🧩 FIX: if another tooltip is mid-fade-out, stop it right away
        StopOtherFadeIfRunning();
    }

    public void OnPointerExit(PointerEventData eventData)
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
    private void StartFadeAndSlideTooltip(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndSlideTooltipCoroutine(targetAlpha));
    }

    private IEnumerator FadeAndSlideTooltipCoroutine(float targetAlpha)
    {
        var tooltip = ui.statToolTip;
        CanvasGroup canvasGroup = tooltip.GetComponent<CanvasGroup>();
        RectTransform rectTransform = tooltip.GetComponent<RectTransform>();

        if (canvasGroup == null || rectTransform == null)
        {
            if (targetAlpha == 0f)
                tooltip.HideStatToolTip();
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + (targetAlpha > 0 ? slideDistance : -slideDistance), startPos.z);

        if (targetAlpha > 0f)
        {
            tooltip.ShowStatToolTip(statDescription);
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
            tooltip.HideStatToolTip();
    }

    private void StopOtherFadeIfRunning()
    {
        // If another slot was fading out the tooltip, stop it
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // Force tooltip to visible if it's mid-fade-out
        var tooltip = ui.statToolTip;
        CanvasGroup cg = tooltip.GetComponent<CanvasGroup>();
        if (cg != null && cg.alpha < 1f)
            cg.alpha = 1f;
    }
}
