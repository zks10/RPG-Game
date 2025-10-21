using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    public Transform attackCheck;
    public float attackCheckRadius = 0.8f;
    private Transform closestEnemy;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update() 
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0) 
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0) {
                Destroy(gameObject);
            }
    }
    public void SetUpClone(Transform newTransform, float cloneDuration, bool canAttack)  
    {
        if (canAttack) 
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;

        FaceClosetTarget();
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
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosetTarget() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, 25);

        float closestDist = Mathf.Infinity;

        foreach (var hit in colliders) 
        {
            if (hit.GetComponent<Enemy>() != null) 
            {
                float distToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distToEnemy < closestDist) 
                    closestDist = distToEnemy;
                    closestEnemy = hit.transform;
            }
        }

        if (closestEnemy != null)  
        {
            if (transform.position.x > closestEnemy.position.x) {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
