using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    [Header("Settings")]
    public float magnetRadius = 2.5f;     // distance where coin starts flying to player
    public float homingSpeed = 10f;       // fly speed
    public float pickupDistance = 0.2f;   // auto-pickup threshold

    private Rigidbody2D rb;
    private Transform player;
    private int currency;

    private bool homing = false;

    public void Init(int amount)
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.player.transform;
        currency = amount;

        // NORMAL PHYSICS at start
        rb.gravityScale = 4;
        rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        float dist = Vector2.Distance(transform.position, player.position);

        // ⭐ Start homing when player is close enough
        if (!homing && dist <= magnetRadius)
        {
            homing = true;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        // ⭐ Homing movement
        if (homing)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * homingSpeed;

            // Pickup
            if (dist <= pickupDistance)
            {
                PlayerManager.instance.currency += currency;
                Destroy(gameObject);
            }
        }
    }
}
