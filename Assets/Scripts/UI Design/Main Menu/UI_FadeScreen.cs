using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn() => anim.SetTrigger("FadeIn");
    public void FadeOut() => anim.SetTrigger("FadeOut");
}
