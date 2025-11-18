using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; 
using System.Collections;


public class UI_StatSlot : UI_Slots
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;    
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField] private string statDescription;

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

    protected override void Start()
    {
        base.Start();
        UpdateStatValueUI();
    }

    public override void ShowToolTip()
    {
        base.ShowToolTip();
        ui.statToolTip.ShowStatToolTip(statDescription);
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
        tooltip = ui.statToolTip;
    }

    public override void HideToolTip()
    {
        base.HideToolTip();
        tooltip.HideToolTips();
    }

    public override void ToolTipShowToolTip()
    {
        base.ToolTipShowToolTip();
        tooltip.ShowToolTips(statDescription);
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
