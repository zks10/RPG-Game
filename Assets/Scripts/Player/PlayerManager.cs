using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
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

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
