using UnityEngine;
using System.Collections;
using Cinemachine;
using TMPro;

public class EntityFx : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    [Header("Pop Up Text FX")]
    [SerializeField] private GameObject popUpTextPrefab;
    [Header("Screen Shake FX")]
    private CinemachineImpulseSource screenShake;
    public Vector3 catchSwordShake;
    public Vector3 highDamageShake;
    public Vector3 counterAttackShake;
    public Vector3 deathShake;
    [SerializeField] private float shakeMultiplier = 1f;
    private float shakeCooldown = 0.05f;
    private float shakeTimer;

    [Header("After Image FX")]
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    private float afterImageTimer;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;
    

    [Header("Ailment Colors")]
    [SerializeField] private Color[] freezeColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment Particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject[] hitFXPrefab;
    
    [Space]
    
    [SerializeField] private ParticleSystem dustFX;
    private GameObject myHealthBar;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        player = PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>(); 
        myHealthBar = GetComponentInChildren<HealthBarUI>(true)?.gameObject;
    }
    private void Update()
    {
        afterImageTimer -= Time.deltaTime;
        shakeTimer -= Time.deltaTime;

    }

    public void CreatePopUpText(
        string text,
        Color color = default,
        float sizeMultiplier = 1f,
        FontStyles fontStyle = FontStyles.Normal,
        bool usePunch = false,
        bool useSquash = false,
        bool useCurve = false)
    {
        if (color == default) color = Color.white;

        Vector3 spawnPos = transform.position + 
                        new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(1f, 1.5f), 0);

        GameObject obj = Instantiate(popUpTextPrefab, spawnPos, Quaternion.identity);

        TextMeshPro tmp = obj.GetComponent<TextMeshPro>();
        tmp.fontSize *= sizeMultiplier;
        tmp.fontStyle = fontStyle;

        obj.GetComponent<PopUpTextFX>()
        .Setup(text, color, 2f, 2f, usePunch, useSquash, useCurve);
    }




    public void ScreenShake(Vector3 _shakePower)
    {
        if (shakeTimer > 0)
            return;

        shakeTimer = shakeCooldown;
        Vector3 impulse = new Vector3(
            _shakePower.x * player.facingDir,
            _shakePower.y,
            0
        ) * shakeMultiplier;

        screenShake.GenerateImpulse(impulse);
    }



    public void CreateAfterImage()
    {
        if (afterImageTimer < 0)
        {
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetUpAfterImage(colorLooseRate, sr.sprite);
            afterImageTimer = afterImageCooldown;
        }
    }
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            //myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            //myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white; 
        yield return new WaitForSecondsRealtime(.2f);
        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColourBlink() {
        if (sr.color != Color.white) 
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }
    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void FreezeFxFor(float _seconds)
    {
        chillFx.Play();
        InvokeRepeating("FreezeColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    
    private void FreezeColorFx()
    {
        if (sr.color != freezeColor[0])
            sr.color = freezeColor[0];
        else
            sr.color = freezeColor[1];
    }

    public void CreateHitFX(Transform _target, bool _crit)
    {
        float zRot = Random.Range(-90, 90);
        float xPos = Random.Range(-0.5f, 0.5f);
        float yPos = Random.Range(-0.5f, 0.5f);

        GameObject hitPrefab = hitFXPrefab[0];
        Vector3 hitFXRot = new Vector3(0, 0, zRot);

        if (_crit)
        {
            hitPrefab = hitFXPrefab[1];
            float yOffset = 0;
            zRot = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yOffset = 180;
            
            hitFXRot = new Vector3(0, yOffset, zRot);

        }

        GameObject newHitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPos, yPos), Quaternion.identity);

        newHitFX.transform.Rotate(hitFXRot);

        Destroy(newHitFX, .5f);
    }

    public void PlayDustFX()
    {
        if (dustFX != null) 
            dustFX.Play();
    }


}
