using UnityEngine;
using TMPro;

public class CoffeeCounter : MonoBehaviour
{
    public TextMeshProUGUI coffeeDisplay;
    public ParticleSystem coffeeParticle;

    public int totalCoffeesSold = 0;
    public GameObject coffeeTrophy;
    private bool trophyAwarded = false;

    public ParticleSystem coffeeTrophyParticle;

    public void RegisterSale()
    {
        totalCoffeesSold++;
        UpdateUI();
        if (coffeeParticle != null)
        {
            coffeeParticle.Stop();
            coffeeParticle.Play();
        }
        CheckTrophy();
    }

    void CheckTrophy()
    {
        if (!trophyAwarded && totalCoffeesSold >= 100)
        {
            trophyAwarded = true;
            coffeeTrophy.SetActive(true);
            if (coffeeTrophyParticle != null)
            {
                coffeeTrophyParticle.Stop();
                coffeeTrophyParticle.Play();
            }
            Debug.Log("Coffee Trophy Unlocked!");
        }
    }

    void UpdateUI()
    {
        coffeeDisplay.text = "Coffees Sold: " + totalCoffeesSold;
    }
}