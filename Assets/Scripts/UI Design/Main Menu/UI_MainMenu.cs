using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private Button continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject confirmationPopUp;
    
    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
        confirmationPopUp.SetActive(true);
    }
    private void Start()
    {
        // Disable Continue if no save
        if (!SaveManager.instance.HasSavedData())
            continueButton.interactable = false;
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        if (SaveManager.instance.HasSavedData())
        {
            UI_ConfirmationPopup.Instance.Show(
                "Start New Game?",
                "Your current progress will be lost. Continue?",
                onConfirm: () =>
                {
                    SaveManager.instance.DeleteSaveData();
                    StartCoroutine(LoadSceneWithFadeEffect(1.5f));
                }
            );
        }
        else
        {
            StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        if (fadeScreen == null)
            Debug.LogError("FadeScreen reference is missing!");
        else
            fadeScreen.FadeOut();

        yield return new WaitForSecondsRealtime(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
