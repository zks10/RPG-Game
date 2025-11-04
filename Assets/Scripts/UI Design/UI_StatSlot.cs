using UnityEngine;
using TMPro;


public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;    
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

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
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
    }
}
