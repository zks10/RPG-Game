using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("Skill UI Images")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image counterAttackImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordThrowImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image trinketImage;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount = 0;
    [SerializeField] private float increaseRate = 200;

    private SkillManager skills;
    private bool crystalUICoolingDown = false;


    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
            UpdateHealthUI();
        }
        skills = SkillManager.instance;
        UpdateSkillUIVisibility(); // show only unlocked skills
        UpdateSoulsUI();

    }

    private void Update()
    {
        UpdateSoulsUI();
        UpdateHealthUI();

        UpdateImageCooldownUI();

        UpdateSkillUIVisibility();

    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrentCurrency())
        {
            soulsAmount += increaseRate * Time.deltaTime;
        }
        else
            soulsAmount = PlayerManager.instance.GetCurrentCurrency();
        currentSouls.text = ((int)soulsAmount).ToString();

    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHP();
        slider.value = playerStats.currentHP;
        hpText.text = playerStats.currentHP + " / " + playerStats.GetCalculatedStatValue(StatType.HP).ToString();
    }

    private void UpdateImageCooldownUI()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && skills.dash.dashUnlocked && skills.dash.cooldownTimer > 0)
            SetCoolDownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.counterAttack.counterAttackUnlocked && PlayerManager.instance.player.counterAttackUsageTimer > 0)
            SetCoolDownOf(counterAttackImage);

        UpdateCrystalCooldownUI();

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlock)
            SetCoolDownOf(swordThrowImage);

        if (Input.GetKeyDown(KeyCode.E) && skills.blackhole.blackholeUnlock && skills.blackhole.IsSkillUsable())
        {
            SetCoolDownOf(blackHoleImage);
        }


        if (Input.GetKeyDown(KeyCode.R) && Inventory.instance.GetEquipmentByType(EquipmentType.Trinket) != null)
            SetCoolDownOf(trinketImage);

        CheckCooldown(dashImage, skills.dash.cooldown);
        CheckCooldown(counterAttackImage, skills.counterAttack.cooldown);
        CheckCooldown(crystalImage, skills.crystal.cooldown);
        CheckCooldown(swordThrowImage, skills.sword.cooldown);
        CheckCooldown(blackHoleImage, skills.blackhole.cooldown);
        CheckCooldown(trinketImage, Inventory.instance.trinketCooldown);
    }

    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldown(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }

    private void UpdateCrystalCooldownUI()
    {
        var crystalSkill = skills.crystal;

        if (crystalSkill.cooldownTimer > 0)
        {
            crystalImage.fillAmount = Mathf.Clamp01(crystalSkill.cooldownTimer / crystalSkill.cooldown);
            crystalUICoolingDown = true;
        }
        else if (crystalUICoolingDown)
        {
            crystalImage.fillAmount = 0;
            crystalUICoolingDown = false;
        }
    }

    
    private void UpdateSkillUIVisibility()
    {
        if (skills == null) return;

        // Enable/disable skill UI based on unlocks
        if (dashImage != null)
            dashImage.transform.parent.gameObject.SetActive(skills.dash.dashUnlocked);

        if (counterAttackImage != null)
            counterAttackImage.transform.parent.gameObject.SetActive(skills.counterAttack.counterAttackUnlocked);

        if (crystalImage != null)
            crystalImage.transform.parent.gameObject.SetActive(skills.crystal.crystalUnlocked);

        if (swordThrowImage != null)
            swordThrowImage.transform.parent.gameObject.SetActive(skills.sword.swordUnlock);

        if (blackHoleImage != null)
            blackHoleImage.transform.parent.gameObject.SetActive(skills.blackhole.blackholeUnlock);

        // Trinket: visible only if equipped
        if (trinketImage != null)
        {
            bool hasTrinketEquipped = Inventory.instance.GetEquipmentByType(EquipmentType.Trinket) != null;
            trinketImage.transform.parent.gameObject.SetActive(hasTrinketEquipped);
        }
    }
}
