using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private bool isQuitting = false;

    [Header("General SFX Settings")]
    [SerializeField] private float sfxMinimalDistance = 10f;

    [Header("All SFX Clips (Player + Enemies + World)")]
    [SerializeField] private AudioSource[] sfx;

    [Header("BGM")]
    [SerializeField] private AudioSource[] bgm;
    private int bgmIndex;
    public bool playBGM = true;
    private bool canPlaySFX;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1);
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
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public void PlaySFX(int index, Transform source = null)
    {
        if (index < 0 || index >= sfx.Length) return;

        if (canPlaySFX == false)
            return;
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
        if (index < 0 || index >= sfx.Length)
            return;

        AudioSource src = sfx[index];

        if (!src)    
            return;

        src.Stop();
    }
    /* -------------------------------------------------------------------------
        Recommended Fade Durations for AudioSource Fade-Out
        ---------------------------------------------------
        Use these values when calling FadeOutSFX(audio, duration)

        1. Very Short SFX  (hits, UI clicks, footsteps, sword swings)
        - Duration: 0.05f to 0.15f
        - Reason: These sounds are brief; long fades feel unnatural.

        2. Medium SFX  (enemy growls, attack charges, ability sounds)
        - Duration: 0.15f to 0.35f
        - Reason: Slight fade helps smooth transitions without feeling slow.

        3. Ambient / Environmental Loops  (wind, rain, fire, water, area sounds)
        - Duration: 0.3f to 0.7f
        - Reason: Smooth fade avoids abrupt audio pops when entering/exiting areas.

        4. Music / BGM
        - Duration: 1.0f to 2.0f
        - Reason: Longer fade creates natural musical transitions.

        5. General-Purpose Default Fade (safe for most SFX)
        - Recommended: 0.25f
        - Balanced, smooth, and works for most gameplay sounds.

        Notes:
        - Extremely short fades (<0.05s) may cause "pops".
        - Extremely long fades (>2s) feel slow unless used for music.
        - Use linear fades (Lerp) for consistent fading speed.
        - Always restore initial volume after stopping the sound.
        ------------------------------------------------------------------------- */
    public void PlaySFXWithTime(int _idx, float _duration)
    {
        if (isQuitting || this == null) return;
        if (!gameObject) return;

        StartCoroutine(FadeInSFX(sfx[_idx], _duration));
    }


    public void StopSFXWithTime(int _idx, float _duration)
    {
        if (isQuitting || this == null) return;
        if (!gameObject) return;

        StartCoroutine(FadeOutSFX(sfx[_idx], _duration));
    }

    private IEnumerator FadeInSFX(AudioSource audio, float duration)
    {
        if (audio == null) yield break;

        float targetVolume = audio.volume;  // store intended final volume
        audio.volume = 0f;                  // start from 0
        if (!audio.isPlaying)
            audio.Play();

        float t = 0f;
        while (t < duration && audio)
        {
            t += Time.deltaTime;
            audio.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        if (audio)
            audio.volume = targetVolume; // ensure final volume is correct
    }
    private IEnumerator FadeOutSFX(AudioSource audio, float duration)
    {
        if (audio == null) yield break;
        if (!audio.isPlaying) yield break;

        float startVolume = audio.volume;
        float t = 0f;

        while (t < duration && audio)
        {
            t += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        if (audio)
        {
            audio.Stop();
            audio.volume = startVolume; // restore original volume
        }
    }



    private void AllowSFX() => canPlaySFX = true;

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
