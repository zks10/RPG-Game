using UnityEngine;
using UnityEngine.UI;

public class DoubleJumpSkill : Skill
{
    [Header("Double Jump")]
    [SerializeField] private UI_SkillTreeSlot unlockDJumpButton;
    public bool dJumpUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDJumpButton.onSkillUnlocked.AddListener(UnlockDJump);
    }

    protected override void CheckUnlock()
    {
        UnlockDJump();
    }
    private void UnlockDJump()
    {
        if (unlockDJumpButton.unlocked && !dJumpUnlocked)
        {
            dJumpUnlocked = true;
            PlayerManager.instance.player.canDoubleJump = true;
        }
    }

}
