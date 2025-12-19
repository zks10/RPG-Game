using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Void Grasp Effect", menuName = "Items Data/Item Effects/Void Grasp")]
public class VoidGraspEffect : ItemEffect
{
    [Header("Charge")]
    public float holdSeconds = 2f;

    [Header("Cooldown")]
    public float cooldownSeconds = 3f;

    [Header("Targeting")]
    public int targets = 3;
    public float range = 5f;
    public LayerMask whatIsEnemy;

    [Header("Spawn")]
    public GameObject handPrefab;
    public Vector2 spawnOffset = new Vector2(0f, 1.2f);

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.user == null) return;

        CharacterStats casterStats = ctx.user.GetComponent<CharacterStats>();
        if (casterStats == null) return;

        // Find enemies in range
        var hits = Physics2D.OverlapCircleAll(ctx.user.position, range, whatIsEnemy);

        var enemies = hits
            .Select(h => h.GetComponent<EnemyStats>())
            .Where(es => es != null && !es.isDead)
            .OrderBy(es => Vector2.Distance(ctx.user.position, es.transform.position))
            .Take(targets);

        foreach (var es in enemies)
        {
            Vector3 pos = es.transform.position + (Vector3)spawnOffset;
            GameObject hand = Instantiate(handPrefab, pos, Quaternion.identity);

            // Use your merged controller
            HandSpell_Controller ctrl = hand.GetComponent<HandSpell_Controller>();
            if (ctrl != null)
                ctrl.SetUpSpell(casterStats);
        }
    }
}
