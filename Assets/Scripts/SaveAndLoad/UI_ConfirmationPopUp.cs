using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_ConfirmationPopup : MonoBehaviour
{
    public static UI_ConfirmationPopup Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private System.Action onConfirm;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string title, string message, System.Action onConfirm)
    {
        this.onConfirm = onConfirm;

        titleText.text = title;
        messageText.text = message;

        panel.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            this.onConfirm?.Invoke();
            Close();
        });

        cancelButton.onClick.AddListener(() => Close());
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
