using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength;
    public Stat maxHP;
    public Stat damage;
    [SerializeField] private int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP.GetValue();

    }
    public virtual void TakeDamage(int _damage)
    {
        currentHP -= _damage;

        if (currentHP <= 0)
            Die();
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }
    protected virtual void Die()
    {

    }
    
    protected virtual void Update()
    {
        
    }
}
