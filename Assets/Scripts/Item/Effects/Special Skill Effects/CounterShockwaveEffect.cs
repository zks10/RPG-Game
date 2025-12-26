using UnityEngine;

[CreateAssetMenu(fileName = "Counter Shockwave Effect", menuName = "Items Data/Item Effects/Counter Shockwave")]
public class CounterShockwaveEffect : ItemEffect
{
    [Header("Spawn")]
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private Vector2 offsetFacingRight = new Vector2(1.60386f, -0.421283f);

    [SerializeField] private float lifeTime = 1;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null || shockwavePrefab == null) return;

        float dir = 1f;

        Player p = ctx.user.GetComponent<Player>();
        if (p != null)
            dir = p.facingDir;

        Vector2 offset = new Vector2(offsetFacingRight.x * dir, offsetFacingRight.y);

        Vector3 pos = ctx.user.position + (Vector3)offset;
        pos.z = 0f;

        GameObject vfx = Instantiate(shockwavePrefab, pos, Quaternion.identity);
        Destroy(vfx, lifeTime);

    }
}
