using UnityEngine;
using System.Collections.Generic;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleAmountOfItem;
    [SerializeField] private ItemData[] possibleDrops;
    private List<ItemData> dropList = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrops()
    {
        if (possibleDrops == null || possibleDrops.Length == 0)
            return;

        dropList.Clear();

        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrops[i].dropRate)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        if (dropList.Count == 0)
            return;

        for (int i = 0; i < possibleAmountOfItem; i++)
        {
            if (dropList.Count == 0)
                break; 

            int randomIndex = Random.Range(0, dropList.Count);
            ItemData randomItem = dropList[randomIndex];

            dropList.RemoveAt(randomIndex);
            DropItem(randomItem);
        }
    }


    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-7, 7), Random.Range(10, 13));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
