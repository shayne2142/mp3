using UnityEngine;
using System.Collections;

public class CoffeeMachine : MonoBehaviour
{
    public float brewTime = 3.0f; // 1.5f for upgraded
    public float coffeeValue = 5.0f; // 8.0f for upgraded
    public bool isAutomatic = false; // true for upgraded

    public GameObject fincan;
    public GameObject kahve;
    public GameObject kurabiye;
    public GameObject tabak;
    public ParticleSystem coffeeParticle;
    
    private bool isBrewing = false;
    private bool coffeeReady = false;
    private MoneyCounter moneyManager;
    private CoffeeCounter coffeeCount;
    [Header("coffeeAudio")]
    public AudioSource coffeeAudioSource; // The speaker in the 3D world
    public AudioClip coffeePurchaseSound;     // The  building sound

    void Start()
    {
        moneyManager = Object.FindFirstObjectByType<MoneyCounter>();
        coffeeCount = Object.FindFirstObjectByType<CoffeeCounter>();
        SetVisualState("Idle");
    }

    public void OnMachineClicked()
    {
        if (!isBrewing && !coffeeReady)
        {
            StartCoroutine(BrewCoffee());
        }
        else if (coffeeReady)
        {
            SellCoffee();
        }
    }

    IEnumerator BrewCoffee()
    {
        isBrewing = true;
        SetVisualState("Brewing");
        Debug.Log("Brewing...");
        
        yield return new WaitForSeconds(brewTime);
        
        isBrewing = false;
        coffeeReady = true;
        SetVisualState("Ready");
        Debug.Log("Coffee is Ready! Click to sell.");

        if (coffeeParticle != null)
        {
            coffeeParticle.Stop();
            coffeeParticle.Play();
        }

        if (isAutomatic)
        {
            yield return new WaitForSeconds(0.5f);
            SellCoffee();
            yield return new WaitForSeconds(0.5f);
            OnMachineClicked(); 
        }
    }

    void SellCoffee()
    {
        moneyManager.AddMoney(coffeeValue);
        coffeeCount.RegisterSale();
        coffeeReady = false;
        SetVisualState("Idle");
        Debug.Log("Coffee Sold!");
        if (coffeeAudioSource != null && coffeePurchaseSound != null)
        {
            coffeeAudioSource.PlayOneShot(coffeePurchaseSound);
        }
    }

    void SetVisualState(string state)
    {
        tabak.SetActive(true);
        kurabiye.SetActive(true);

        if (state == "Idle")
        {
            fincan.SetActive(false);
            kahve.SetActive(false);
        }
        else if (state == "Brewing")
        {
            fincan.SetActive(true);
            kahve.SetActive(false);
        }
        else if (state == "Ready")
        {
            fincan.SetActive(true);
            kahve.SetActive(true);
        }
    }

    void SetAutomatic(bool automatic)
    {
        isAutomatic = automatic;
    }

    public void multiplyValue(float multiplier)
    {
        coffeeValue *= multiplier;
    }
}