using UnityEngine;

[CreateAssetMenu(fileName = "Fire Burn Effect", menuName = "Items Data/Item Effects/Fire Burn")]
public class FireBurnEffect : ItemEffect
{
    [SerializeField] private GameObject fireBurnPrefab;
    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject newFireBurn = Instantiate(fireBurnPrefab, _enemyTransform.position, Quaternion.identity, _enemyTransform);

        newFireBurn.transform.localPosition = Vector3.zero;

        Destroy(newFireBurn, PlayerManager.instance.player.GetComponent<PlayerStats>().GetAilmentDuration());
    }
}
