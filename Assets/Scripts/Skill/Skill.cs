using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Update() 
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill() 
    {
        if (cooldownTimer < 0) 
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

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
}
