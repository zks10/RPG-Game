using System.Collections.Generic;
using UnityEngine;


public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturn;
    private float freezeTimeDuration;
    private float returnSpeed ;
    private bool returnRotation = true;

    [Header("Bounce Info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Pierce Info")]
    private float pierceAmount;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;
    private bool firstHit;
    private Vector2 launchDirection;
    private bool spinSoundPlaying;


    private float destorySwordDistance = 20;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        enemyTarget = new List<Transform>();
    }
    
    private void DestroyMe()
    {
        if(Vector2.Distance(transform.position, player.transform.position) > destorySwordDistance)
            Destroy(gameObject);
    }

    public void SetUpSword(Vector2 _dir, float _gravity, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravity;
        player = _player;
        returnSpeed = _returnSpeed;
        freezeTimeDuration = _freezeTimeDuration;
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        launchDirection = _dir.normalized;
        Invoke("DestroyMe", 2);
    }

    public void SetUpBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
    }

    public void SetUpPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetUpSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        firstHit = false;
    }
    
    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturn)
        {
            if (returnRotation)
            {
                anim.SetBool("Rotation", true);
                returnRotation = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();


    }

    private void BounceLogic()  
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturn = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            transform.position += (Vector3)(launchDirection * 1.5f * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
                StopWhenSpinning();
            
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                if (spinTimer < 0)
                {
                    isReturn = true;
                    isSpinning = false;
                    AudioManager.instance.StopSFX(23); 
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            if (hit.GetComponent<EnemyStats>().isDead) continue;
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturn = true;
        firstHit = false;
        spinSoundPlaying = false;
        AudioManager.instance.StopSFX(23);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInto(collision);

    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        if (hit.GetComponent<EnemyStats>().isDead) continue;
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            if (!firstHit)
            {
                StopWhenSpinning();
                firstHit = true;
            }
            return;
        }

        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing && enemyTarget.Count > 0)
            return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
        if (!spinSoundPlaying)
        {
            AudioManager.instance.PlaySFX(23, null);
            spinSoundPlaying = true;
        }
    }
    
    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        player.stats.DoPhysicalDamage(enemyStats);

        if (player.skill.sword.timeStopUnlock)
            enemy.FreezeTimeFor(freezeTimeDuration);

        if (player.skill.sword.volunerableUnlock)
            enemyStats.MakeVolunerableFor(freezeTimeDuration); 

        ItemData_Equipment equipAmulet = Inventory.instance.GetEquipmentByType(EquipmentType.Amulet);
        if (equipAmulet != null)
            equipAmulet.ItemEffect(enemy.transform);
    }
}
