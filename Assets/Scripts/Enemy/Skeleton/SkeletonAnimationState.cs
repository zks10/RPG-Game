using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton skeleton => GetComponentInParent<EnemySkeleton>();
    private void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position, skeleton.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage();
        }
    }
}
