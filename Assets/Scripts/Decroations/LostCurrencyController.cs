using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int currency;
    public GameObject retrievePrompt;

    private bool playerIsInRange = false;

    private void Start()
    {
        if (retrievePrompt != null)
            retrievePrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerIsInRange = true;
            retrievePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerIsInRange = false;
            retrievePrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerIsInRange && Input.GetKeyDown(KeyCode.T))
        {
            PlayerManager.instance.currency += currency;
            AudioManager.instance.PlaySFX(38, transform);
            retrievePrompt.SetActive(false);
            Destroy(gameObject);
        }
    }
}
