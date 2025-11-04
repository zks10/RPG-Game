using UnityEngine;
using UnityEngine.UI;


public class UI_BackpackUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject inventoryPanel;
    public GameObject ediblePanel;
    public GameObject stashPanel;

    [Header("Buttons")]
    public Button inventoryButton;
    public Button edibleButton;
    public Button materialButton; 

    private Color normalColor = new Color(1f, 1f, 1f, 0.45f);
    private Color activeColor = new Color(1f, 1f, 1f, 1f);

    private void Start()
    {
        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        ediblePanel.SetActive(false);
        stashPanel.SetActive(false);
        HighlightButton(inventoryButton);
    }

    public void ShowEdible()
    {
        inventoryPanel.SetActive(false);
        ediblePanel.SetActive(true);
        stashPanel.SetActive(false);
        HighlightButton(edibleButton);
    }

    public void ShowMaterial()
    {
        inventoryPanel.SetActive(false);
        ediblePanel.SetActive(false);
        stashPanel.SetActive(true);
        HighlightButton(materialButton);
    }

    private void HighlightButton(Button active)
    {
        // Reset all to normal color
        inventoryButton.image.color = normalColor;
        edibleButton.image.color = normalColor;
        materialButton.image.color = normalColor;

        // Highlight the active one
        active.image.color = activeColor;
    }
    
}
