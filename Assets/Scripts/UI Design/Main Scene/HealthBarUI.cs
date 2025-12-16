using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform myTransform;
    private Slider slider;
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats.onDeath += DisableHealthBar;
        slider.maxValue = myStats.GetMaxHP();

        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        slider.value = myStats.currentHP;
        slider.maxValue = myStats.GetMaxHP();
    }
    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }
    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
        
        if (myStats == null)
            return;
        myStats.onHealthChanged -= UpdateHealthUI;
        myStats.onDeath -= DisableHealthBar;
    }
    private void DisableHealthBar()
    {
        gameObject.SetActive(false); 
    }
}
