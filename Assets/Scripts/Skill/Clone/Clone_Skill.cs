using UnityEngine;
using System.Collections;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashEnd;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Chance to Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead of Clone")]
    public  bool crystalInsteadOfClone;



    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().
            SetUpClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(player.transform), canDuplicateClone, chanceToDuplicate, player);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }
    
    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
