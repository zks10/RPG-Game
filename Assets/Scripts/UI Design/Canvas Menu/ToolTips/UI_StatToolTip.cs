using UnityEngine;
using TMPro;

public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI description;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ShowStatToolTip(string _text)
    {
        description.text = _text;

        gameObject.SetActive(true);
        UpdatePosition(true);
    }

    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdatePosition(bool instant = false)
    {
        base.UpdatePosition(instant);
    }

    public override void ShowInstantlyAtMouse()
    {
        base.ShowInstantlyAtMouse();
    }

    public override void HideToolTips()
    {
        base.HideToolTips();
        HideStatToolTip();
    }
    public override void ShowToolTips(string description)
    {
        ShowStatToolTip(description);
    }
}
