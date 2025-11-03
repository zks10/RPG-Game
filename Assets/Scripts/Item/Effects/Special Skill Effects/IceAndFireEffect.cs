using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Items Data/Item Effects/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = (player.primaryAttackState.comboCounter == 2);
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().linearVelocity = newVelocity * player.facingDir;

            Destroy(newIceAndFire, 6);
        }
        
    }
}
