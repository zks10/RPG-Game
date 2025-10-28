using UnityEngine;
using System.Collections;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(.2f);

        sr.material = originalMat;
    }

    private void RedColourBlink() {
        if (sr.color != Color.white) 
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelRedBlink() {
        CancelInvoke();
        sr.color = Color.white;
    }
}
