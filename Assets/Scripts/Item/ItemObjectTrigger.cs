using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{

    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
                return;

            //if (collision.GetComponent<Player>().canPickItem)
            myItemObject.PickUpItem();
        }
    }


}
