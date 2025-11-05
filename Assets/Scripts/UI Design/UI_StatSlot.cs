using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; 

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;    
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private UI ui;


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

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        UpdateStatValueUI();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
         ui.statToolTip.HideStatToolTip();
    }
}
