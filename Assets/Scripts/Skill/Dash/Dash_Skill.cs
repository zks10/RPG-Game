using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Dash Clone")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;

    [Header("Dash Clone on Arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockkButton;

    protected override void Start() 
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDashClone);
        cloneOnArrivalUnlockkButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
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
