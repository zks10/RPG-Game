using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Items Data/Item Effects/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null || ctx.target == null) return;

        Player player = ctx.user.GetComponent<Player>();
        if (player == null) return;

        bool thirdAttack = (player.primaryAttackState.comboCounter == 2);
        if (!thirdAttack) return;

        GameObject obj = Instantiate(iceAndFirePrefab, ctx.target.position, player.transform.rotation);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = newVelocity * player.facingDir;

        Destroy(obj, 6f);
    }
}
