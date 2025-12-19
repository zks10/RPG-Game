using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float xVelocity;

    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private bool flipped;
    [SerializeField] private bool canMove;
    private Rigidbody2D rb;
    private CharacterStats myStats;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {  
        if (canMove)
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (collision.TryGetComponent<Entity>(out var entity))
            {
                entity.SetUpKnockBackDir(transform);
                entity.lockKnockbackDir = true;
                if (!collision.GetComponent<CharacterStats>().isDead)
                {
                    myStats.DoPhysicalDamage(collision.GetComponent<CharacterStats>());
                    myStats.DoMagicalDamage(collision.GetComponent<CharacterStats>());
                }
            }

            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
        }
    }

    public void FlipArrow(string _targetLayerName = "Enemy")
    {
        xVelocity *= -1;
        transform.Rotate(0, 180, 0);
        canMove = true;
        transform.position += new Vector3(Mathf.Sign(xVelocity) * 0.2f, 0, 0);
        rb.linearVelocity = new Vector2(xVelocity, 0);
        targetLayerName = _targetLayerName;
    }
    public void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<BoxCollider2D>().enabled = false;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
        Destroy(gameObject, Random.Range(2, 3.5f));
    }

    public void SetUpArrow(float _speed, CharacterStats _myStats)
    {
        xVelocity = _speed;
        myStats = _myStats;
        
    }
}
