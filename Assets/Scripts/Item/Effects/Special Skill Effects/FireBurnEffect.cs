using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Fire Burn Effect", menuName = "Items Data/Item Effects/Fire Burn")]
public class FireBurnEffect : ItemEffect
{
    [SerializeField] private GameObject fireBurnPrefab;
    [SerializeField] private float spawnInterval = 0.7f;
    [SerializeField] private float jiggleRadius = 0.1f;

    public override void ExecuteEffect(EffectContext ctx)
    {
        if (ctx.target == null) return;

        EnemyStats enemyStats = ctx.target.GetComponent<EnemyStats>();
        if (enemyStats == null || enemyStats.isDead) return;

        CoroutineRunner runner = ctx.target.GetComponent<CoroutineRunner>();
        if (runner == null)
            runner = ctx.target.gameObject.AddComponent<CoroutineRunner>();

        runner.StartCoroutine(SpawnFireBurns(ctx.target));
    }

    private IEnumerator SpawnFireBurns(Transform enemyTransform)
    {
        if (enemyTransform == null) yield break;

        float duration = PlayerManager.instance.player.GetComponent<PlayerStats>().GetAilmentDuration();
        float elapsed = 0f;

        EnemyStats enemyStats = enemyTransform.GetComponent<EnemyStats>();

        while (elapsed < duration)
        {
            if (enemyTransform == null) yield break;
            if (enemyStats != null && enemyStats.isDead) yield break;

            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0f
            );

            GameObject flame = Instantiate(fireBurnPrefab, enemyTransform.position + randomOffset, Quaternion.identity, enemyTransform);

            CoroutineRunner runner = flame.GetComponent<CoroutineRunner>();
            if (runner == null)
                runner = flame.AddComponent<CoroutineRunner>();

            runner.StartCoroutine(FadeAndSmoothJiggle(flame, spawnInterval, jiggleRadius));

            yield return new WaitForSeconds(spawnInterval);

            if (flame != null)
                Destroy(flame);

            elapsed += spawnInterval;
        }
    }

    private IEnumerator FadeAndSmoothJiggle(GameObject flame, float duration, float radius)
    {
        if (flame == null) yield break;

        SpriteRenderer sr = flame.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(flame);
            yield break;
        }

        Color color = sr.color;
        Vector3 startPos = flame.transform.localPosition;
        float half = duration / 2f;
        float elapsed = 0f;

        while (elapsed < half)
        {
            if (flame == null) yield break;

            color.a = Mathf.Lerp(0f, 1f, elapsed / half);
            sr.color = color;

            float jiggleX = (Mathf.PerlinNoise(Time.time * 5f, 0f) - 0.5f) * 2f * radius;
            float jiggleY = (Mathf.PerlinNoise(0f, Time.time * 5f) - 0.5f) * 2f * radius;
            flame.transform.localPosition = startPos + new Vector3(jiggleX, jiggleY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        color.a = 1f;
        sr.color = color;

        while (elapsed < half)
        {
            if (flame == null) yield break;

            color.a = Mathf.Lerp(1f, 0f, elapsed / half);
            sr.color = color;

            float jiggleX = (Mathf.PerlinNoise(Time.time * 5f, 0f) - 0.5f) * 2f * radius;
            float jiggleY = (Mathf.PerlinNoise(0f, Time.time * 5f) - 0.5f) * 2f * radius;
            flame.transform.localPosition = startPos + new Vector3(jiggleX, jiggleY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (flame != null)
            Destroy(flame);
    }
}

public class CoroutineRunner : MonoBehaviour { }
