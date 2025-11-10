using UnityEngine;

public class UI : MonoBehaviour
{
    // [SerializeField] private GameObject characterUI; 0
    // [SerializeField] private GameObject skillUI; 1
    // [SerializeField] private GameObject craftUI; 2
    // [SerializeField] private GameObject optionsUI; 3
    [SerializeField] private GameObject[] menuUI = new GameObject[4];
    private int menuIdx;
    private bool openMenu;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;
    void Start()
    {
        SwitchTo(null);
        openMenu = false;

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            openMenu = !openMenu;

            if (openMenu)
                SwitchWithKeyTo(menuUI[menuIdx]);
            else
                SwitchWithKeyTo(null);
        }

        if (openMenu)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                menuIdx = (menuIdx + 1) % menuUI.Length;
                SwitchWithKeyTo(menuUI[menuIdx]);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                menuIdx = (menuIdx - 1 + menuUI.Length) % menuUI.Length;
                
                SwitchWithKeyTo(menuUI[menuIdx]);
            }
        }
        

       
        
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }
        
    }
    
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }
        SwitchTo(_menu);
    }
}
