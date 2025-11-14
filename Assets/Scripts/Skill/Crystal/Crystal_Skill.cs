using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;
    public bool waitTeleport { get; private set; }

    [Header("Crystal Simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneInsteadlButton;
    public bool cloneInsteadOfCrystal { get; private set; }


    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    public bool canExplode { get; private set; }
    [SerializeField] private float growSpeed;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    public bool canMoveToEnemy { get; private set; }
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackCrystalButton;
    public bool canUseMultiStacks { get; private set; }
    [SerializeField] private int amountOfStacks;
    [SerializeField] public float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        waitTeleport = false;
        unlockCrystalButton.onSkillUnlocked.AddListener(UnlockCrystal);
        unlockCloneInsteadlButton.onSkillUnlocked.AddListener(UnlockCloneInstead);
        unlockExplosiveButton.onSkillUnlocked.AddListener(UnlockExplosive);
        unlockMovingCrystalButton.onSkillUnlocked.AddListener(UnlockMovingCrystal);
        unlockMultiStackCrystalButton.onSkillUnlocked.AddListener(UnlockMultiStackCrystal);
    }

    #region Unlock Region
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked && !crystalUnlocked)
            crystalUnlocked = true;
    }

    private void UnlockCloneInstead()
    {
        if (unlockCloneInsteadlButton.unlocked && !cloneInsteadOfCrystal)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockExplosive()
    {
        if (unlockExplosiveButton.unlocked && !canExplode)
            canExplode = true;
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked && !canMoveToEnemy)
            canMoveToEnemy = true;
    }

    private void UnlockMultiStackCrystal()
    {
        if (unlockMultiStackCrystalButton.unlocked && !canUseMultiStacks)
            canUseMultiStacks = true;
    }
    #endregion

    public override bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            if ((crystalUnlocked || cloneInsteadOfCrystal) && !canMoveToEnemy && !canUseMultiStacks)
            {
                if (waitTeleport && currentCrystal != null)
                {
                    UseSkill();
                    cooldownTimer = cooldown;
                    waitTeleport = false;
                    return true;
                }
                
                if (!waitTeleport)
                {
                    UseSkill();
                    waitTeleport = true;
                    return true;
                }
            }
            else
            {
                UseSkill();
                cooldownTimer = cooldown; 
                return true;
            }
        }
        return false;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (canUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }

    }
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(currentCrystal.transform), player);

    }
    public void OnCrystalDestroyed()
    {
        if (waitTeleport)
        {
            waitTeleport = false;
            cooldownTimer = cooldown; 
        }

        currentCrystal = null;
    }


    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool canUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                    
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                
                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, growSpeed, FindClosestEnemy(newCrystal.transform), player);
            
                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
            return true;
            }
        }
        return false;
    }

    private void RefillCrystal()
    {
        int amount = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amount; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        cooldown = multiStackCooldown;
        cooldownTimer = multiStackCooldown;

        RefillCrystal();
    }


}
