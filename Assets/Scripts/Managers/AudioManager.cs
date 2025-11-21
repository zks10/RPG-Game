using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("General SFX Settings")]
    [SerializeField] private float sfxMinimalDistance = 10f;

    [Header("All SFX Clips (Player + Enemies + World)")]
    [SerializeField] private AudioSource[] sfx;

    [Header("BGM")]
    [SerializeField] private AudioSource[] bgm;
    private int bgmIndex;
    public bool playBGM = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        if (!playBGM)
        {
            StopALLBGM();
            return;
        }

        if (!bgm[bgmIndex].isPlaying)
            PlayBGM(bgmIndex);
    }
    public void PlaySFX(int index, Transform source = null)
    {
        if (index < 0 || index >= sfx.Length) return;

        // Distance check (for enemies)
        if (source != null)
        {
            float dist = Vector2.Distance(
                PlayerManager.instance.player.transform.position,
                source.position
            );

            if (dist > sfxMinimalDistance)
                return;
        }

        sfx[index].pitch = Random.Range(0.9f, 1.1f);
        sfx[index].Play();
    }

    public void StopSFX(int index)
    {
        if (index >= 0 && index < sfx.Length)
            sfx[index].Stop();
    }

    // ---------------------------------------------------------
    // BGM
    // ---------------------------------------------------------
    public void PlayBGM(int index)
    {
        bgmIndex = index;
        StopALLBGM();

        if (index < bgm.Length)
            bgm[index].Play();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopALLBGM()
    {
        foreach (var track in bgm)
            track.Stop();
    }
}
