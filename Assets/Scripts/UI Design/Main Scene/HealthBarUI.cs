using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private Slider slider;
    private CharacterStats myStats;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = entity.GetComponent<CharacterStats>();
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
        myStats.onDeath += DisableHealthBar;
        slider.maxValue = myStats.GetMaxHP();
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.value = myStats.currentHP;
    }
    private void FlipUI() => myTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
        myStats.onDeath -= DisableHealthBar;
    }
    private void DisableHealthBar()
    {
        gameObject.SetActive(false); 
    }
}
