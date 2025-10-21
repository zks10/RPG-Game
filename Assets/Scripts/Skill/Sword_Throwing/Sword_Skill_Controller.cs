
using UnityEngine;


public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturn;
    [SerializeField] private float returnSpeed = 20; 
    private void Awake() 
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

    }

    public void SetUpSword(Vector2 _dir, float _gravity, Player _player)
    {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravity;
        player = _player;
        anim.SetBool("Rotation", true);  
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //anim.SetBool("Rotation", true);  
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                //anim.SetBool("Rotation", false);
                player.CatchTheSword();
        }

    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturn = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn)
            return;
        anim.SetBool("Rotation", false);
        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;

    }
}
