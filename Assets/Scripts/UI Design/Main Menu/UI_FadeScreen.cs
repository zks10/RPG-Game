using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn() => anim.SetBool("IsFadeIn", false);
    public void FadeOut() => anim.SetBool("IsFadeIn", true);
}
