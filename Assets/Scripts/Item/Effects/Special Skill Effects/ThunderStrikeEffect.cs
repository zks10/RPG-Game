using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Items Data/Item Effects/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 0.5f);
    }
}
