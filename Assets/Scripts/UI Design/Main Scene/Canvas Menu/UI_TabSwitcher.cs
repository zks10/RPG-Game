using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_TabSwitcher : MonoBehaviour
{
    [Header("Optional Panels (leave empty if you only want button highlight)")]
    public List<GameObject> panels;

    [Header("Buttons (required)")]
    public List<Button> buttons;

    [Header("Colors")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.45f);
    public Color activeColor = new Color(1f, 1f, 1f, 1f);

    private int currentIndex = -1;

    private void Start()
    {
        // Assign click listeners
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // Capture index for closure
            buttons[i].onClick.AddListener(() =>
            {
                ShowPanel(index);
                HighlightButton(index);
            });
        }

        // Default to first button highlight if desired
        if (buttons.Count > 0)
            HighlightButton(0);

        // Optionally show first panel if present
        if (panels.Count > 0)
            ShowPanel(0);
    }

    /// <summary>
    /// Show only the selected panel (if panels exist).
    /// </summary>
    public void ShowPanel(int index)
    {
        if (panels == null || panels.Count == 0)
            return;

        for (int i = 0; i < panels.Count; i++)
            panels[i].SetActive(i == index);

        currentIndex = index;
    }

    /// <summary>
    /// Highlight a button visually without toggling any panels.
    /// </summary>
    public void HighlightButton(int index)
    {
        if (buttons == null || buttons.Count == 0)
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.color = (i == index) ? activeColor : normalColor;
        }
    }
}
