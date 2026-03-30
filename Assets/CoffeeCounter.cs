using UnityEngine;
using TMPro;

public class CoffeeCounter : MonoBehaviour
{
    public TextMeshProUGUI coffeeDisplay;
    public int totalCoffeesSold = 0;
    public GameObject coffeeTrophy;
    private bool trophyAwarded = false;

    public void RegisterSale()
    {
        totalCoffeesSold++;
        UpdateUI();
        CheckTrophy();
    }

    void CheckTrophy()
    {
        if (!trophyAwarded && totalCoffeesSold >= 100)
        {
            trophyAwarded = true;
            coffeeTrophy.SetActive(true);
            Debug.Log("Coffee Trophy Unlocked!");
        }
    }

    void UpdateUI()
    {
        coffeeDisplay.text = "Coffees Sold: " + totalCoffeesSold;
    }
}