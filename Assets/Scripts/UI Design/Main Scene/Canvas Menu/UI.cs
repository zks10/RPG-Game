using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISaveManager
{
    // [SerializeField] private GameObject characterUI; 0
    // [SerializeField] private GameObject skillUI; 1
    // [SerializeField] private GameObject craftUI; 2
    // [SerializeField] private GameObject optionsUI; 3
    [SerializeField] private GameObject[] menuUI = new GameObject[4];
    [SerializeField] private GameObject inGameUI;
    
    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject saveButton;
    [SerializeField] private GameObject returnMainMenuButton;


    private int menuIdx;
    private bool openMenu;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(inGameUI);
        openMenu = false;

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        
        foreach (UI_VolumeSlider vs in volumeSettings)
        {
            vs.SliderValue(vs.slider.value);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            
            openMenu = !openMenu;

            AudioManager.instance.PlaySFX(16,null);
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
        menuIdx = index;
        AudioManager.instance.PlaySFX(14, null);              
        SwitchTo(menuUI[index]);     

    }

    public void SwitchTo(GameObject menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadedScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!isFadedScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else if (menu != null)
                GameManager.instance.PauseGame(true);

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
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    public IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        endText.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);
        restartButton.SetActive(true);
    }


    public void RestartGame() => GameManager.instance.RestartScene();
    public void SaveGame() => SaveManager.instance.SaveGame();
    public void ReturnMainMenuGame()
    {
        SaveManager.instance.SaveGame();
        GameManager.instance.PauseGame(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
