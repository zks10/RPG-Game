using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Fire Burn Effect", menuName = "Items Data/Item Effects/Fire Burn")]
public class FireBurnEffect : ItemEffect
{
    [SerializeField] private GameObject fireBurnPrefab;
    [SerializeField] private float spawnInterval = 0.7f; // Duration each prefab stays
    [SerializeField] private float jiggleRadius = 0.1f;  // Smaller radius for subtle movement

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        CoroutineRunner runner = _enemyTransform.GetComponent<CoroutineRunner>();
        if (runner == null)
        {
            runner = _enemyTransform.gameObject.AddComponent<CoroutineRunner>();
        }
        runner.StartCoroutine(SpawnFireBurns(_enemyTransform));
    }

    private IEnumerator SpawnFireBurns(Transform enemyTransform)
    {
        float duration = PlayerManager.instance.player.GetComponent<PlayerStats>().GetAilmentDuration();
        float elapsed = 0f;
        EnemyStats enemyStats = enemyTransform.GetComponent<EnemyStats>();

        while (elapsed < duration)
        {
            if (enemyStats != null && enemyStats.isDead)
            {
                yield break; // Stop spawning flames immediately
            }
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0f
            );

            GameObject newFireBurn = Instantiate(fireBurnPrefab, enemyTransform.position + randomOffset, Quaternion.identity, enemyTransform);

            CoroutineRunner runner = newFireBurn.GetComponent<CoroutineRunner>();
            if (runner == null)
                runner = newFireBurn.AddComponent<CoroutineRunner>();

            runner.StartCoroutine(FadeAndSmoothJiggle(newFireBurn, spawnInterval, jiggleRadius));

            yield return new WaitForSeconds(spawnInterval);

            Destroy(newFireBurn);

            elapsed += spawnInterval;
        }
    }

    private IEnumerator FadeAndSmoothJiggle(GameObject flame, float duration, float radius)
    {
        SpriteRenderer sr = flame.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(flame);
            yield break;
        }

        Color color = sr.color;
        Vector3 startPos = flame.transform.localPosition;
        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // Fade in
        while (elapsed < halfDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsed / halfDuration);
            sr.color = color;

            // Smooth jiggle with Perlin noise for subtle movement
            float jiggleX = (Mathf.PerlinNoise(Time.time * 5f, 0f) - 0.5f) * 2f * radius;
            float jiggleY = (Mathf.PerlinNoise(0f, Time.time * 5f) - 0.5f) * 2f * radius;
            flame.transform.localPosition = startPos + new Vector3(jiggleX, jiggleY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        sr.color = color;
        elapsed = 0f;

        // Fade out while continuing smooth jiggle
        while (elapsed < halfDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, elapsed / halfDuration);
            sr.color = color;

            float jiggleX = (Mathf.PerlinNoise(Time.time * 5f, 0f) - 0.5f) * 2f * radius;
            float jiggleY = (Mathf.PerlinNoise(0f, Time.time * 5f) - 0.5f) * 2f * radius;
            flame.transform.localPosition = startPos + new Vector3(jiggleX, jiggleY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(flame);
    }
}

public class CoroutineRunner : MonoBehaviour { }
