using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    private CharacterStats myStats;
    [SerializeField] private float growSpeed = 15;
    [SerializeField] private float maxSize = 6;
    [SerializeField] private bool canGrow;
    private float explosionRadius;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }
    public void SetUpExplosive(CharacterStats _myStats, float _growSpeed, float _maxSize, float _radius) 
    {

        myStats = _myStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _radius;
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PlayerStats>().isDead) continue;
                myStats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                hit.GetComponent<Entity>().SetUpKnockBackDir(transform);

                
            }
        }
        
    }
    private void SelfDestroy() => Destroy(gameObject);
}
