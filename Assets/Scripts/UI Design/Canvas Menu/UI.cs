using UnityEngine;

public class UI : MonoBehaviour
{
    // [SerializeField] private GameObject characterUI; 0
    // [SerializeField] private GameObject skillUI; 1
    // [SerializeField] private GameObject craftUI; 2
    // [SerializeField] private GameObject optionsUI; 3
    [SerializeField] private GameObject[] menuUI = new GameObject[4];
    [SerializeField] private GameObject inGameUI;
    private int menuIdx;
    private bool openMenu;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;
    void Start()
    {
        SwitchTo(inGameUI);
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
                SwitchWithKeyTo(inGameUI);
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

    public void SwitchToIndex(int index)
    {
        menuIdx = index;               // sync index
        SwitchTo(menuUI[index]);       // show the correct menu
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
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }
    
    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(inGameUI);
    }
}
