using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activeStatus;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (string.IsNullOrEmpty(id))
            GenerateId();
    }
    [ContextMenu("Genrate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!activeStatus)
                ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint() 
    {
        if (!activeStatus)
            AudioManager.instance.PlaySFX(8, transform);
            
        activeStatus = true;
        anim.SetBool("Active", true);
        GameManager.instance.SetLastCheckpoint(id);
    }
}
