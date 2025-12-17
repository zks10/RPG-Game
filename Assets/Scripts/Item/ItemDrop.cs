using UnityEngine;
using System.Collections.Generic;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxAmountItemsToDrop;
    [SerializeField] private ItemData[] possibleDrops;
    // private List<ItemData> dropList = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrops()
    {
        if (possibleDrops == null || possibleDrops.Length == 0)
            return;

        for (int i = 0; i < maxAmountItemsToDrop; i++)
        {
            ItemData droppedItem = RollForItem();
            if (droppedItem != null)
            {
                DropItem(droppedItem);
            }
        }
    }

    private ItemData RollForItem()
    {
        foreach (ItemData item in possibleDrops)
        {
            if (Random.Range(0f, 100f) < item.dropRate)
            {
                return item;
            }
        }
        return null;
    }
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-7, 7), Random.Range(10, 13));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
