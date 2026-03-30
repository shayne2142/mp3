using UnityEngine;
using TMPro;

public class VendingCount : MonoBehaviour
{
    public TextMeshProUGUI vendingDisplay;
    public float totalVendingsSold = 0f;
    public float passiveRate = 0.1f;
    public GameObject vendingTrophy;
    private bool trophyAwarded = false;

    public void Update()
    {
        totalVendingsSold += passiveRate * Time.deltaTime;
        UpdateUI();
        CheckTrophy();
    }

    void CheckTrophy()
    {
        if (!trophyAwarded && totalVendingsSold >= 1000f)
        {
            trophyAwarded = true;
            vendingTrophy.SetActive(true);
            Debug.Log("Vending Trophy Unlocked!");
        }
    }

    public void RegisterSale(float count)
    {
        totalVendingsSold += count;
    }

    public void SetZero()
    {
        totalVendingsSold = 0f;
    }

    void UpdateUI()
    {
        vendingDisplay.text = "Vendings Sold: " + totalVendingsSold.ToString("F1");
    }
}