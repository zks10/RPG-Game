using UnityEngine;
using UnityEngine.UI; 

public class CounterAttack_Skill : Skill
{
    [Header("Counter Attack")]
    [SerializeField] private UI_SkillTreeSlot counterAttackUnlockButton;
    public bool counterAttackUnlocked { get; private set; }

    [Header("Counter Attack Restore")]
    [SerializeField] private UI_SkillTreeSlot restoreCounterAttackButton;
    public bool restoreCounterAttackUnlocked { get; private set; }
    [Range(.1f, 1f)]
    [SerializeField] private float restoreHPAmountPercentage;

    [Header("Counter Attack Mirage")]
    [SerializeField] private UI_SkillTreeSlot mirageCounterAttackButton;
    public bool mirageCounterAttackUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
        if (restoreCounterAttackUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHP() * restoreHPAmountPercentage);
            player.stats.IncreaseHPBy(restoreAmount);
        }
    }
    protected override void Start()
    {
        base.Start();
        cooldown = player.counterAttackCooldown;
        counterAttackUnlockButton.onSkillUnlocked.AddListener(UnlockCoutnerAttack);
        restoreCounterAttackButton.onSkillUnlocked.AddListener(UnlockCoutnerAttackRestore);
        mirageCounterAttackButton.onSkillUnlocked.AddListener(UnlockMirageCoutnerAttack);
    }

    private void UnlockCoutnerAttack()
    {
        if (counterAttackUnlockButton.unlocked && !counterAttackUnlocked)
            counterAttackUnlocked = true;
    }

    private void UnlockCoutnerAttackRestore()
    {
        if (restoreCounterAttackButton.unlocked && !restoreCounterAttackUnlocked)
            restoreCounterAttackUnlocked = true;
    }

    private void UnlockMirageCoutnerAttack()
    {
        if (mirageCounterAttackButton.unlocked && !mirageCounterAttackUnlocked)
            mirageCounterAttackUnlocked = true;
    }

    public void MakeMirageOnCounterAttack(Transform _respawnTransform) 
    {
        if (mirageCounterAttackUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }


}
