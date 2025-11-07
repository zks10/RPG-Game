using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "Skill Name Slot - " + skillName;
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());

    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Can not unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Can not unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowSkillToolTip(skillDescription, skillName);
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
         ui.skillToolTip.HideSkillToolTip();
    }
}
