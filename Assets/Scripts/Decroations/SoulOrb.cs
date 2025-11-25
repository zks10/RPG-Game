using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    [Header("Settings")]
    public float magnetRadius = 2.5f;    
    public float homingSpeed = 10f;       // fly speed
    public float pickupDistance = 0.4f;   // auto-pickup threshold

    private Rigidbody2D rb;
    private Transform player;
    private int currency;

    private bool homing = false;

    public float homingDelay = 0.5f; 
    private float timer = 0f; 

    public void Init(int amount)
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        currency = amount;

        rb.gravityScale = 4;
        rb.bodyType = RigidbodyType2D.Dynamic;

    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (player == null)
            return;
        
        if (timer < homingDelay)
            return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!homing && dist <= magnetRadius)
        {
            homing = true;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        if (homing)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * homingSpeed;

            if (dist <= pickupDistance)
            {
                AudioManager.instance.PlaySFX(9, transform);
                PlayerManager.instance.currency += currency;
                Destroy(gameObject);
            }
        }
    }
}
