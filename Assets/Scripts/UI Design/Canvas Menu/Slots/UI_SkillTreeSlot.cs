using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UI_SkillTreeSlot : UI_Slots
{
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "Skill Name Slot - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
    }

    protected override void Start()
    {
        base.Start();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
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

    public override void ShowToolTip()
    {
        base.ShowToolTip();
        ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName);
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
        tooltip = ui.skillToolTip;
    }

    public override void HideToolTip()
    {
        base.HideToolTip();
        tooltip.HideToolTips();
    }

    public override void ToolTipShowToolTip()
    {
        base.ToolTipShowToolTip();
        tooltip.ShowToolTips(skillDescription, skillName);
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
