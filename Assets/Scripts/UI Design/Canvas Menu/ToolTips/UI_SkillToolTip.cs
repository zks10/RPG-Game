using UnityEngine;
using TMPro;
public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;


    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowSkillToolTip(string _skillDescription, string _skillName)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        
        gameObject.SetActive(true);
        UpdatePosition(true);
    }

    public void HideSkillToolTip()
    {
        skillText.text = "";
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

    public override void HideToolTips()
    {
        base.HideToolTips();
        HideSkillToolTip();
    }
    public override void ShowToolTips(string skillDesc, string skillName)
    {
        ShowSkillToolTip(skillDesc, skillName);
    }
}
