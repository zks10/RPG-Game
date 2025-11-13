using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;
    public int currency;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
            return false;
        return true;
    }

    public void UpdateCurrency(int _price)
    {
        currency -= _price;
    }
    
    public int GetCurrentCurrency() { return currency; }
}
