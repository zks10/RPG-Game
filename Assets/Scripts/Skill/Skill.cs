using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }
    protected virtual void Update() 
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {
        
    }
    public virtual bool CanUseSkill() 
    {
        if (IsSkillUsable()) 
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        player.fx.CreatePopUpText("Cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        player.SetSkillActive(true);
    }
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDist = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<EnemyStats>().isDead) continue;
                float distToEnemy = Vector2.Distance(_checkTransform.transform.position, hit.transform.position);

                if (distToEnemy < closestDist)
                {
                    closestDist = distToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }

    public virtual bool IsSkillUsable()
    {
        return cooldownTimer <= 0 && !player.isSkillActive;
    }

}
