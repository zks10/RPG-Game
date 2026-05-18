using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Decrease Armor Effect", menuName = "Items Data/Item Effects/Decrease Armor")]
public class ArmorDecreaseEffect : ItemEffect
{
    [SerializeField] private int armorDecreasePerHit = 2;
    [SerializeField] private int maxArmorDecrease = 4;
    [SerializeField] private float duration = 2f;

    private Dictionary<CharacterStats, int> currentDecrease = new Dictionary<CharacterStats, int>();
    private Dictionary<CharacterStats, float> lastHitTime = new Dictionary<CharacterStats, float>();

    public override void ExecuteEffect(EffectContext context)
    {
        CharacterStats targetStats = context.target.GetComponent<CharacterStats>();

        if (targetStats == null)
            return;

        if (!currentDecrease.ContainsKey(targetStats))
            currentDecrease[targetStats] = 0;

        int newDecrease = Mathf.Min(currentDecrease[targetStats] + armorDecreasePerHit, maxArmorDecrease);
        int extraDecrease = newDecrease - currentDecrease[targetStats];

        if (extraDecrease > 0)
        {
            targetStats.armor.AddModifier(-extraDecrease);
            currentDecrease[targetStats] = newDecrease;
        }

        lastHitTime[targetStats] = Time.time;

        targetStats.StartCoroutine(RemoveArmorAfterDelay(targetStats, lastHitTime[targetStats]));

        Debug.Log("Armor decreased: -" + currentDecrease[targetStats]);
    }

    private IEnumerator RemoveArmorAfterDelay(CharacterStats targetStats, float hitTime)
    {
        yield return new WaitForSeconds(duration);

        if (targetStats == null)
            yield break;

        if (!lastHitTime.ContainsKey(targetStats))
            yield break;

        if (lastHitTime[targetStats] != hitTime)
            yield break;

        if (currentDecrease[targetStats] > 0)
        {
            targetStats.armor.RemoveModifier(-currentDecrease[targetStats]);
            currentDecrease[targetStats] = 0;
        }

        Debug.Log("Armor decrease removed");
    }
}