using UnityEngine;
using System.Collections;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] freezeColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white; 
        yield return new WaitForSeconds(.2f);
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
    }
    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void FreezeFxFor(float _seconds)
    {
        
        InvokeRepeating("FreezeColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
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
}
