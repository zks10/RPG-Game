using UnityEngine;
using UnityEngine.Audio;


public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;
    [SerializeField] private float fadeDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFXWithTime(areaSoundIndex, fadeDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.StopSFXWithTime(areaSoundIndex, fadeDuration);
        }
    }
}
