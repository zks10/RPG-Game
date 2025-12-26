using UnityEngine;

[CreateAssetMenu(menuName = "Items Data/Item Effects/Vortex Pull On Spin Tick")]
public class VortexPullOnSpinTickEffect : ItemEffect
{
    [Header("Pull")]
    [SerializeField] private float radius = 3.2f;
    [SerializeField] private float pullForce = 18f;          // tune this
    [SerializeField] private float maxForce = 25f;           // clamp
    [SerializeField] private float closeBoost = 1.35f;       // stronger near center
    [SerializeField] private LayerMask enemyMask;

    //i[Header("Boss / Heavy handling")]
    //[SerializeField] private bool affectBosses = false;
    //[SerializeField] private float bossPullMultiplier = 0.25f;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.trigger != ItemTrigger.OnSwordSpinTick)
            return;

        if (ctx.user == null)
            return;

        Vector2 center = ctx.user.position;

        // ✅ Do NOT use enemyMask while testing (mask is the #1 reason hits=0)
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);

        foreach (var h in hits)
        {
            Enemy enemy = h.GetComponent<Enemy>();
            if (enemy == null) continue;

            EnemyStats stats = h.GetComponent<EnemyStats>();
            if (stats != null && stats.isDead) continue;

            Rigidbody2D er = h.attachedRigidbody;
            if (er == null) continue;

            Vector2 toCenter = center - (Vector2)h.transform.position;
            float dist = toCenter.magnitude;
            if (dist <= 0.02f) continue;

            float t = 1f - Mathf.Clamp01(dist / radius);
            float speed = pullForce * Mathf.Lerp(1f, closeBoost, t); // treat pullForce as "speed"
            speed = Mathf.Min(speed, maxForce);

            // ✅ strong pull: directly move rigidbody toward center
            Vector2 step = toCenter.normalized * speed * Time.deltaTime;
            er.MovePosition(er.position + step);
        }
    }

}
