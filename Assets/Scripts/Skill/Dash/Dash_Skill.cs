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
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash() { 
        Debug.Log("Attempt");
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockDashClone() {
        cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival() {
        cloneOnArrivalUnlocked = true; 
    }
}
