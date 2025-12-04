
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlock { get; private set; }
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;

    Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }
    
    private void UnlockBlackHole()
    {
        if (blackholeUnlockButton.unlocked && !blackholeUnlock)
            blackholeUnlock = true;
    }
    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
        blackholeUnlockButton.onSkillUnlocked.AddListener(UnlockBlackHole);
    }

    protected override void CheckUnlock()
    {
        UnlockBlackHole();
    }
    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;

    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
