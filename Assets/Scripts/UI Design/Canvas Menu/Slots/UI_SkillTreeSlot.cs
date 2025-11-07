using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;

    private bool isHovering = false;
    private bool isTooltipVisible = false;
    private Coroutine fadeCoroutine;
    private Coroutine hoverDelayCoroutine;

    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float slideDistance = 10f;
    [SerializeField] private float hoverDelay = 0.2f;

    private void OnValidate()
    {
        gameObject.name = "Skill Name Slot - " + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        foreach (var s in shouldBeUnlocked)
        {
            if (!s.unlocked)
            {
                Debug.Log("Cannot unlock skill: prerequisite not met.");
                return;
            }
        }

        foreach (var s in shouldBeLocked)
        {
            if (s.unlocked)
            {
                Debug.Log("Cannot unlock skill: conflicting skill is active.");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
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

            ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName);
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

    // === Fade + Slide Animation ===
    private void StartFadeAndSlideTooltip(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndSlideTooltipCoroutine(targetAlpha));
    }

    private IEnumerator FadeAndSlideTooltipCoroutine(float targetAlpha)
    {
        var tooltip = ui.skillToolTip;
        CanvasGroup canvasGroup = tooltip.GetComponent<CanvasGroup>();
        RectTransform rectTransform = tooltip.GetComponent<RectTransform>();

        if (canvasGroup == null || rectTransform == null)
        {
            if (targetAlpha == 0f)
                tooltip.HideSkillToolTip();
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + (targetAlpha > 0 ? slideDistance : -slideDistance), startPos.z);

        if (targetAlpha > 0f)
        {
            tooltip.ShowSkillToolTip(skillDescription, skillName);
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
            tooltip.HideSkillToolTip();
    }

    private void StopOtherFadeIfRunning()
    {
        // If another slot was fading out the tooltip, stop it
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // Force tooltip to visible if it's mid-fade-out
        var tooltip = ui.skillToolTip;
        CanvasGroup cg = tooltip.GetComponent<CanvasGroup>();
        if (cg != null && cg.alpha < 1f)
            cg.alpha = 1f;
    }
}
