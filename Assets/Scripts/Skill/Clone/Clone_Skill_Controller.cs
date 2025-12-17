using UnityEngine;
using System.Collections;


public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    public Transform attackCheck;
    public float attackCheckRadius = 0.8f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float chanceToDuplicate;
    private Player player;
    private float attackMultiplier;
    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyRadiusCheck = 25;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(FaceClosetTarget());
    }
    private void Update() 
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 _offset, bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)  
    {
        if (canAttack) 
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        transform.position = newTransform.position + _offset;
        cloneTimer = cloneDuration;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;
        attackMultiplier = _attackMultiplier;


    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<EnemyStats>().isDead) continue;
                //player.stats.DoPhysicalDamage(hit.GetComponent<CharacterStats>());
                hit.GetComponent<Entity>().SetUpKnockBackDir(transform);
                
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weapon = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
                    if (weapon != null)
                        weapon.ItemEffect(hit.transform); 
                }
                
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private IEnumerator FaceClosetTarget() 
    {
        yield return null;

        FindClosestEnemy();

        if (closestEnemy != null) 
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, closestEnemyRadiusCheck, whatIsEnemy);

        float closestDist = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<EnemyStats>().isDead) continue;
            float distToEnemy = Vector2.Distance(transform.position, hit.transform.position);

            if (distToEnemy < closestDist)
            {
                closestDist = distToEnemy;
                closestEnemy = hit.transform;
            }
            
        }
    }

}
