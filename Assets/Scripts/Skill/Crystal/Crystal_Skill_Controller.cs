using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float growSpeed;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _growSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            int random = Random.Range(0, colliders.Length);
            closestTarget = colliders[random].transform;
        }
    }
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

        if (canMove)
        {
            if (closestTarget == null)
                return;
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 0.5)
            {
                FinishCrystal();
                canMove = false;
            } 
                
        }
           
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipAmulet = Inventory.instance.GetEquipementByType(EquipmentType.Amulet);
                if (equipAmulet != null)
                    equipAmulet.ItemEffect(hit.transform);
                
            }
        }
        
    }
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
            
        else
            SelfDestroy();
    }
    public void SelfDestroy() => Destroy(gameObject);
}
