using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton skeleton => GetComponentInParent<EnemySkeleton>();
    private void AnimationTrigger()
    {
        skeleton.AnimationFinishTrigger();
    }
}
