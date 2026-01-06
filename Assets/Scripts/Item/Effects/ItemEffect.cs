using UnityEngine;
public enum ItemTrigger
{
    None,
    OnHitEnemy,
    OnTakeDamage,
    OnUse,
    OnCounter,
    OnSwordSpinTick,
    OnEquip,
    OnQueryCrystalMaxStack 
}

public struct EffectContext
{
    public ItemTrigger trigger;
    public Transform user; 
    public Transform target;       
}


public class ItemEffect : ScriptableObject
{
    public ItemTrigger trigger = ItemTrigger.None;   

    public virtual void ExecuteEffect(EffectContext ctx)
    {

    }
    
}
