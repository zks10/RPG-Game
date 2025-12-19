using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterStats>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        if (myStats == null || slider == null)
        {
            Debug.LogError("HealthBarUI missing references", this);
            enabled = false;
            return;
        }

        slider.maxValue = myStats.GetMaxHP();
        UpdateHealthUI();

        myStats.onDeath += DisableHealthBar;
    }

    private void UpdateHealthUI()
    {
        if (myStats == null || slider == null)
            return;

        slider.maxValue = myStats.GetMaxHP();
        slider.value = myStats.currentHP;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnEnable()
    {
        if (entity != null)
            entity.onFlipped += FlipUI;

        if (myStats != null)
            myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
        {
            myStats.onHealthChanged -= UpdateHealthUI;
            myStats.onDeath -= DisableHealthBar;
        }
    }

    private void DisableHealthBar()
    {
        gameObject.SetActive(false); 
    }
}
