using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    public UI_ItemToolTip itemToolTip;
    void Start()
    {
        itemToolTip.GetComponentInChildren<UI_ItemToolTip>();
    }

    // Update is called once per frame
    void Update()
    {

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
}
