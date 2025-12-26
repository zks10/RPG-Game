using UnityEngine;

public class WeaponChargeController : MonoBehaviour
{
    private float lastTapTime;
    private float lastCastTime = Mathf.NegativeInfinity;

    private const float doubleTapWindow = 0.25f;

    private void Update()
    {
        ItemData_Equipment weapon = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
        if (weapon == null || weapon.itemEffects == null) return;

        VoidGraspEffect effect = null;
        foreach (var e in weapon.itemEffects)
        {
            effect = e as VoidGraspEffect;
            if (effect != null) break;
        }
        if (effect == null) return;

        if (Time.timeScale == 0) return;
        if (Input.GetMouseButtonDown(0))
        {
            float time = Time.time;

            // Double tap detected
            if (time - lastTapTime <= doubleTapWindow &&
                time >= lastCastTime + effect.cooldownSeconds)
            {
                lastCastTime = time;

                weapon.ItemEffect(new EffectContext
                {
                    trigger = ItemTrigger.OnUse,
                    user = transform,
                    target = null
                });
            }

            lastTapTime = time;
        }
    }
}
