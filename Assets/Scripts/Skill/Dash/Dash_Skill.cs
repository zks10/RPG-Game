using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Dash Clone")]
    public bool cloneOnDashUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;

    [Header("Dash Clone on Arrival")]
    public bool cloneOnArrivalUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockkButton;


    protected override void Start() 
    {
        base.Start();

        dashUnlockButton.onSkillUnlocked.AddListener(UnlockDash);
        cloneOnDashUnlockButton.onSkillUnlocked.AddListener(UnlockDashClone);
        cloneOnArrivalUnlockkButton.onSkillUnlocked.AddListener(UnlockCloneOnArrival);
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash() { 
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockDashClone() {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockkButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    
    public void CloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);

    }

    public void CloneOnDashEnd()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
