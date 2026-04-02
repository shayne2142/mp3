using UnityEngine;
using TMPro;

public class VendingCount : MonoBehaviour
{
    public TextMeshProUGUI vendingDisplay;
    public ParticleSystem coffeeParticle;

    public float totalVendingsSold = 0f;
    public float passiveRate = 0.1f;
    public GameObject vendingTrophy;
    private bool trophyAwarded = false;
    private bool hasSetZero = false;

    public ParticleSystem vendingTrophyParticle;

    public void Update()
    {
        totalVendingsSold += passiveRate * Time.deltaTime;
        if (totalVendingsSold >= 900f && !hasSetZero)
        {
            Reset();
        }
        UpdateUI();
        CheckTrophy();
    }

    void CheckTrophy()
    {
        if (!trophyAwarded && totalVendingsSold >= 1000f)
        {
            trophyAwarded = true;
            vendingTrophy.SetActive(true);
            if (vendingTrophyParticle != null)
            {
                vendingTrophyParticle.Stop();
                vendingTrophyParticle.Play();
            }
            Debug.Log("Vending Trophy Unlocked!");
        }
    }

    public void RegisterSale(float count)
    {
        totalVendingsSold += count;
        if (coffeeParticle != null)
        {
            coffeeParticle.Stop();
            coffeeParticle.Play();
        }
    }

    private void Reset() // internal reset so that the trophy doesn't get activated before the player has unlocked vending machines
    {
        totalVendingsSold = 0f;
    }

    public void SetZero()
    {
        totalVendingsSold = 0f;
        hasSetZero = true;
    }

    void UpdateUI()
    {
        vendingDisplay.text = "Vendings Sold: " + totalVendingsSold.ToString("F1");
    }
}