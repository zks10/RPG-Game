using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int strikeDamage;
    private Animator anim;
    private bool triggered;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        strikeDamage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
            return;
        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, 0.32f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
            
        }
    }
    
    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(strikeDamage);
        Destroy(gameObject, .4f);
    }
}
