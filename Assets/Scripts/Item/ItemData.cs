using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
    Edible
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Items Data/Materials")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropRate;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }

}
