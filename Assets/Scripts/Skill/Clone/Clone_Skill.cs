using UnityEngine;
using System.Collections;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    public bool canAttack { get; private set; }

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead of Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone { get; private set; }

    protected override void Start()
    {
        base.Start();
        cloneAttackUnlockButton.onSkillUnlocked.AddListener(CloneAttackUnlock);
        aggressiveCloneUnlockButton.onSkillUnlocked.AddListener(AggressiveCloneUnlock);
        multipleCloneUnlockButton.onSkillUnlocked.AddListener(MultipleCloneUnlock);
        crystalInsteadUnlockButton.onSkillUnlocked.AddListener(CrystalInsteadUnlock);

    }

    #region Unlock Region
    private void CloneAttackUnlock()
    {
        if (cloneAttackUnlockButton.unlocked && !canAttack)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void AggressiveCloneUnlock()
    {
        if (aggressiveCloneUnlockButton.unlocked && !canApplyOnHitEffect)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void MultipleCloneUnlock()
    {
        if (multipleCloneUnlockButton.unlocked && !canDuplicateClone)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneAttackMultiplier;
        }
    }

    private void CrystalInsteadUnlock()
    {
        if (crystalInsteadUnlockButton.unlocked && !crystalInsteadOfClone)
        {
            crystalInsteadOfClone = true;
        }
    }

    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetUpClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(player.transform),
                canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }
    
    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
