using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin

}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    [Header("Regular Info")]
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;


    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetUpGravity();
    }

    private void SetUpGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetUpBounce(true, bounceAmount, bounceSpeed);
        if (swordType == SwordType.Pierce)
            newSwordScript.SetUpPierce(pierceAmount); 
        if (swordType == SwordType.Spin)
            newSwordScript.SetUpSpin(true, maxTravelDistance, spinDuration, hitCooldown); 
            
        newSwordScript.SetUpSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }


    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        //     swordType = SwordType.Regular;
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        //     swordType = SwordType.Bounce;
        // if (Input.GetKeyDown(KeyCode.Alpha3))
        //     swordType = SwordType.Pierce;
        // if (Input.GetKeyDown(KeyCode.Alpha4))
        //     swordType = SwordType.Spin;
    }

    #region Aim Region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePosition - playerPosition;

        return dir;
    }
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position
                            + new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * t
                            + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
