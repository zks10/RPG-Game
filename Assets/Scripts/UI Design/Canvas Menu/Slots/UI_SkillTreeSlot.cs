using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using System.Collections;

public class UI_SkillTreeSlot : UI_Slots, ISaveManager
{
    [SerializeField] private string skillName;
    [SerializeField] private int skillPrice;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;
    public UnityEvent onSkillUnlocked;


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

        if (unlocked) 
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (!PlayerManager.instance.HaveEnoughMoney(skillPrice))
            return;
        
        if (unlocked)
            return;

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
        PlayerManager.instance.UpdateCurrency(skillPrice);
        unlocked = true;
        skillImage.color = Color.white;
        onSkillUnlocked?.Invoke();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else 
            _data.skillTree.Add(skillName, unlocked);
    }




    # region UI of tool tips
    public override void ShowToolTip()
    {
        base.ShowToolTip();
        ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName, skillPrice);
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
        tooltip.ShowToolTips(skillDescription, skillName, skillPrice);
    }

    public override IEnumerator FadeAndSlideTooltipCoroutine(float targetAlpha)
    {
        yield return base.FadeAndSlideTooltipCoroutine(targetAlpha);
    }

    public override void StopOtherFadeIfRunning()
    {
        base.StopOtherFadeIfRunning();
    }
    #endregion

}
