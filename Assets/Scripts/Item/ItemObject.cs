using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private Player player;
    [SerializeField] private float magnetRange = 0.1f;  
    [SerializeField] private float moveSpeed = 5f;  

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;
        SetupVisuals();
    }
    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddEquipItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.linearVelocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }


    private void Update()
    {
        if (player == null || player.isDead)
            return;
        if (!Inventory.instance.CanAddEquipItem() && itemData.itemType == ItemType.Equipment)
            return;
        MagneticMovingToPlayer();
    }

    private void MagneticMovingToPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= magnetRange)
        {
            float magneticForce = Mathf.Clamp01(1f - (distance / magnetRange)); 
            float currentSpeed = moveSpeed * (1f + magneticForce * 5f); 

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                currentSpeed * Time.deltaTime
            );
        }
    }
}
