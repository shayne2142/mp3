using UnityEngine;
using TMPro;
using UnityEngine.XR; 
using System.Collections.Generic;

public class CoffeeShopManager : MonoBehaviour
{
    public GameObject machine2Parent;
    public GameObject machine3Parent;
    public GameObject upgraded1Parent;
    public GameObject upgraded2Parent;

    public GameObject machine1Real;
    public GameObject machine2RealVisual;
    public GameObject machine2Ghost;
    public GameObject machine3RealVisual; 
    public GameObject machine3Ghost;
    public GameObject upgraded1RealVisual;
    public GameObject upgraded1Ghost;
    public GameObject upgraded2RealVisual;
    public GameObject upgraded2Ghost;

    public float priceForMachine2 = 500f;
    public float priceForMachine3 = 1000f;
    public float priceForUpgraded1 = 1500f;
    public float priceForUpgraded2 = 2000f;
    public float priceForWall = 3000f;
    public float priceForVending1 = 4000f;
    public float priceForVending2 = 6500f;
    public float priceForVending3 = 10000f;

    public float passiveNormal = 0.5f;
    public float passiveUpgraded = 5.0f;
    public float passiveVending = 50.0f;

    private MoneyCounter moneySystem;
    private VendingCount vendingSystem;
    private bool machine2Bought = false;
    private bool machine3Bought = false;
    private bool upgraded1Bought = false;
    private bool upgraded2Bought = false;
    private bool wallBought = false;
    private bool vending1Bought = false;
    private bool vending2Bought = false;

    public GameObject realWall;
    public GameObject ghostWall;
    public GameObject wallPurchasePrompt;
    public GameObject vendingStatsDisplay;

    public GameObject vending1;
    public GameObject vending2;
    public GameObject vending3;
    public GameObject vending1Ghost;
    public GameObject vending2Ghost;
    public GameObject vending3Ghost;
    public GameObject vending1Real;
    public GameObject vending2Real;
    public GameObject vending3Real;

    public GameObject Powerups;
    public int PowerupNumber = 0;
    public float[] powerUpPrices = {2000, 10000};
    public TextMeshProUGUI powerUpDisplay;



    [Header("Wall Audio")]
    public AudioSource wallAudioSource; // The speaker in the 3D world
    public AudioClip purchaseSound;     // The  building sound

    [Header("coffee machine Audio")]
    public AudioSource coffeeMachineAudioSource; // The speaker in the 3D world
    public AudioClip coffeeMachinePurchaseSound;     // The  building sound

    [Header("coffee machine upgraded Audio")]
    public AudioSource coffeeMachineUpgradedAudioSource; // The speaker in the 3D world
    public AudioClip coffeeMachineUpgradedPurchaseSound;     // The  building sound

    [Header("vendingAudio")]
    public AudioSource vendingAudioSource; // The speaker in the 3D world
    public AudioClip vendingPurchaseSound;     // The  building sound

    [Header("powerupAudio")]
    public AudioSource powerupAudioSource; // The speaker in the 3D world
    public AudioClip powerupPurchaseSound;     // The  building sound


    [Header("Particle Effects")]
    public ParticleSystem machine2Particles;
    public ParticleSystem machine3Particles;
    public ParticleSystem upgraded1Particles;
    public ParticleSystem upgraded2Particles;
    public ParticleSystem wallParticles;
    public ParticleSystem vending1Particles;
    public ParticleSystem vending2Particles;
    public ParticleSystem vending3Particles;
    public ParticleSystem powerupParticles;
    void Start()
    {
        moneySystem = Object.FindFirstObjectByType<MoneyCounter>();
        vendingSystem = Object.FindFirstObjectByType<VendingCount>();

        machine2RealVisual.SetActive(false);
        machine3RealVisual.SetActive(false);
        
        machine2Ghost.SetActive(true);
        machine3Ghost.SetActive(false);

        upgraded1RealVisual.SetActive(false);
        upgraded2RealVisual.SetActive(false);

        upgraded1Ghost.SetActive(false);
        upgraded2Ghost.SetActive(false);

        realWall.SetActive(true);
        ghostWall.SetActive(false);
        wallPurchasePrompt.SetActive(false);
        vendingStatsDisplay.SetActive(false);
    }
    public void TriggerHapticFeedback(float amplitude, float duration)
    {
        // Get all currently connected VR controllers
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            // Check if the controller supports vibration, then trigger it
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0, amplitude, duration);
            }
        }
    }
    public void AttemptPurchaseMachine2()
    {
        if (moneySystem.moneyCount >= priceForMachine2 && !machine2Bought)
        {
            moneySystem.moneyCount -= priceForMachine2;
            machine2Bought = true;
            machine2RealVisual.SetActive(true);
            machine2Ghost.SetActive(false);

            StartCoroutine(SquishAndBounce(machine2RealVisual.transform));
            machine3Ghost.SetActive(true);
            TriggerHapticFeedback(0.5f, 0.5f);
            moneySystem.AddPassiveRate(passiveNormal);
            if (coffeeMachineAudioSource != null && coffeeMachinePurchaseSound != null)
            {
                coffeeMachineAudioSource.PlayOneShot(coffeeMachinePurchaseSound);
            }
            if (machine2Particles != null) machine2Particles.Play();
        }
        else 
        {
            Debug.Log("Not enough money to buy Machine 2!");
        }
    }

    public void AttemptPurchaseMachine3()
    {
        if (machine2Bought && moneySystem.moneyCount >= priceForMachine3)
        {
            moneySystem.moneyCount -= priceForMachine3;
            machine3Bought = true;
            machine3RealVisual.SetActive(true);
            machine3Ghost.SetActive(false);

            StartCoroutine(SquishAndBounce(machine3RealVisual.transform));
            upgraded1Ghost.SetActive(true);
            TriggerHapticFeedback(0.5f, 0.5f);
            moneySystem.AddPassiveRate(passiveNormal);
            if (coffeeMachineAudioSource != null && coffeeMachinePurchaseSound != null)
            {
                coffeeMachineAudioSource.PlayOneShot(coffeeMachinePurchaseSound);
            }
            if (machine3Particles != null) machine3Particles.Play();
        }
        else if (!machine2Bought)
        {
            Debug.Log("You must buy Machine 2 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Machine 3!");
        }
    }

    public void AttemptPurchaseUpgraded1()
    {
        if (machine3Bought && moneySystem.moneyCount >= priceForUpgraded1)
        {
            moneySystem.moneyCount -= priceForUpgraded1;
            upgraded1Bought = true;
            upgraded1RealVisual.SetActive(true);
            upgraded1Ghost.SetActive(false);
            StartCoroutine(SquishAndBounce(upgraded1RealVisual.transform));
            upgraded2Ghost.SetActive(true);

            moneySystem.AddPassiveRate(passiveUpgraded);
            TriggerHapticFeedback(0.5f, 0.5f);
            if (coffeeMachineUpgradedAudioSource != null && coffeeMachineUpgradedPurchaseSound != null)
            {
                coffeeMachineUpgradedAudioSource.PlayOneShot(coffeeMachineUpgradedPurchaseSound);
            }
            if (upgraded1Particles != null) upgraded1Particles.Play();
        }
        else if (!machine3Bought)
        {
            Debug.Log("You must buy Machine 3 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Upgraded Machine 1!");
        }
    }

    public void AttemptPurchaseUpgraded2()
    {
        if (upgraded1Bought && moneySystem.moneyCount >= priceForUpgraded2)
        {
            moneySystem.moneyCount -= priceForUpgraded2;
            upgraded2Bought = true;
            upgraded2RealVisual.SetActive(true);

            StartCoroutine(SquishAndBounce(upgraded2RealVisual.transform));
            upgraded2Ghost.SetActive(false);
            moneySystem.AddPassiveRate(passiveUpgraded);

            //make the wall ghost
            realWall.SetActive(false);
            ghostWall.SetActive(true);
            wallPurchasePrompt.SetActive(true);
            TriggerHapticFeedback(0.5f, 0.5f);
            if (coffeeMachineUpgradedAudioSource != null && coffeeMachineUpgradedPurchaseSound != null)
            {
                coffeeMachineUpgradedAudioSource.PlayOneShot(coffeeMachineUpgradedPurchaseSound);
            }
            if (upgraded2Particles != null) upgraded2Particles.Play();
        }
        else if (!upgraded1Bought)
        {
            Debug.Log("You must buy Upgraded Machine 1 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Upgraded Machine 2!");
        }
    }

    public void AttemptPurchaseWall()
    {
        if (upgraded2Bought && moneySystem.moneyCount >= priceForWall && !wallBought)
        {
            Debug.Log("Wall purchased!");
            moneySystem.moneyCount -= priceForWall;
            wallBought = true;

            
            StartCoroutine(SquishAndBounce(ghostWall.transform, true, 0.01f, 10f));

            wallPurchasePrompt.SetActive(false);
            vendingStatsDisplay.SetActive(true);

            vending1Ghost.SetActive(true);
            TriggerHapticFeedback(0.9f, 2f);

            if (wallAudioSource != null && purchaseSound != null)
            {
                wallAudioSource.PlayOneShot(purchaseSound);
            }
            if (wallParticles != null) wallParticles.Play();
        }
    }

    public void AttemptPurchaseVending1()
    {
        if (wallBought && moneySystem.moneyCount >= priceForVending1)
        {
            moneySystem.moneyCount -= priceForVending1;
            vending1Bought = true;
            vending1Real.SetActive(true);
            vendingSystem.SetZero();
            vending1Real.GetComponent<SellVending>().ActivateMachine();
            vending1Ghost.SetActive(false);
            StartCoroutine(SquishAndBounce(vending1Real.transform));
            vending2Ghost.SetActive(true);

            moneySystem.AddPassiveRate(passiveVending);

            if (!Powerups.activeSelf && PowerupNumber == 1)
            {
                powerUpDisplay.text = "Vending Sell Price x2 \n Cost: $" + powerUpPrices[1].ToString("F2");
                Powerups.SetActive(true);
            }
            TriggerHapticFeedback(0.5f, 0.5f);
            if (vendingAudioSource != null && vendingPurchaseSound != null)
            {
                vendingAudioSource.PlayOneShot(vendingPurchaseSound);
            }
            if (vending1Particles != null) vending1Particles.Play();
        }
        else if (!wallBought)
        {
            Debug.Log("You must buy Upgraded Machine 2 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Vending Machine 1!");
        }
    }

    public void AttemptPurchaseVending2()
    {
        if (vending1Bought && moneySystem.moneyCount >= priceForVending2)
        {
            moneySystem.moneyCount -= priceForVending2;
            vending2Bought = true;
            vending2Real.SetActive(true);
            vending2Real.GetComponent<SellVending>().ActivateMachine();
            vending2Ghost.SetActive(false);
            StartCoroutine(SquishAndBounce(vending2Real.transform));
            vending3Ghost.SetActive(true);

            moneySystem.AddPassiveRate(passiveVending);
            TriggerHapticFeedback(0.5f, 0.5f);
            if (vendingAudioSource != null && vendingPurchaseSound != null)
            {
                vendingAudioSource.PlayOneShot(vendingPurchaseSound);
            }
            if (vending2Particles != null) vending2Particles.Play();
        }
        else if (!vending1Bought)
        {
            Debug.Log("You must buy Vending Machine 1 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Vending Machine 2!");
        }
    }

    public void AttemptPurchaseVending3()
    {
        if (vending2Bought && moneySystem.moneyCount >= priceForVending3)
        {
            moneySystem.moneyCount -= priceForVending3;
            vending3Real.SetActive(true);
            vending3Real.GetComponent<SellVending>().ActivateMachine();
            vending3Ghost.SetActive(false);
            StartCoroutine(SquishAndBounce(vending3Real.transform));
            moneySystem.AddPassiveRate(passiveVending);
            TriggerHapticFeedback(0.5f, 0.5f);
            if (vendingAudioSource != null && vendingPurchaseSound != null)
            {
                vendingAudioSource.PlayOneShot(vendingPurchaseSound);
            }
            if (vending3Particles != null) vending3Particles.Play();
        }
        else if (!vending2Bought)
        {
            Debug.Log("You must buy Vending Machine 2 first!");
        }
        else 
        {
            Debug.Log("Not enough money to buy Vending Machine 3!");
        }
    }
    
    public void AttemptPurchasePowerUp()
    {
        if (PowerupNumber == 0 && moneySystem.moneyCount >= powerUpPrices[0])
        {
            moneySystem.moneyCount -= powerUpPrices[PowerupNumber];
            PowerupNumber++;

            TriggerHapticFeedback(0.8f, 2f);
            if (powerupAudioSource != null && powerupPurchaseSound != null)
            {
                powerupAudioSource.PlayOneShot(powerupPurchaseSound);
            }
            if (powerupParticles != null) powerupParticles.Play();
            CoffeeMachine[] machines = {machine1Real.GetComponentInChildren<CoffeeMachine>(), 
                                        machine2RealVisual.GetComponentInChildren<CoffeeMachine>(), 
                                        machine3RealVisual.GetComponentInChildren<CoffeeMachine>(), 
                                        upgraded1RealVisual.GetComponentInChildren<CoffeeMachine>(), 
                                        upgraded2RealVisual.GetComponentInChildren<CoffeeMachine>()};

            foreach (CoffeeMachine machine in machines)
            {
                machine.multiplyValue(2.0f);
            }
            
            if (vending1Bought)
            {
                powerUpDisplay.text = "Vending Sell Price x2 \n Cost: $" + powerUpPrices[1].ToString("F2");
            }
            else {
                Powerups.SetActive(false);
            }
            
        }
        else if (PowerupNumber == 0)
        {
            Debug.Log("Not enough money to buy Power Up 1!");
        }
        else if (PowerupNumber == 1 && moneySystem.moneyCount >= powerUpPrices[1])
        {
            moneySystem.moneyCount -= powerUpPrices[1];
            PowerupNumber++;
            TriggerHapticFeedback(0.8f, 2f);
            if (powerupAudioSource != null && powerupPurchaseSound != null)
            {
                powerupAudioSource.PlayOneShot(powerupPurchaseSound);
            }
            if (powerupParticles != null) powerupParticles.Play();
            SellVending[] machines = {vending1Real.GetComponent<SellVending>(), 
                                      vending2Real.GetComponent<SellVending>(), 
                                      vending3Real.GetComponent<SellVending>()};

            foreach (SellVending machine in machines)
            {
                machine.multiplyValue(2.0f);
            }

            Powerups.SetActive(false);
        }
        else if (PowerupNumber == 1)
        {
            Debug.Log("Not enough money to buy Power Up 2!");
        }
        else 
        {
            Debug.Log("All Power Ups bought");
        }
    }

    // This Coroutine handles the math for the bounce animation
    private System.Collections.IEnumerator SquishAndBounce(Transform target, bool disableWhenFinished = false, float squishY = 0.5f, float bulgeXZ = 1.2f)
    {
        Vector3 originalScale = target.localScale;

        
        Vector3 squishedScale = new Vector3(originalScale.x * bulgeXZ, originalScale.y * squishY, originalScale.z * bulgeXZ);

        target.localScale = squishedScale;

        float velocity = 0f;
        float currentValue = 0f;
        float goalValue = 1f;
        float stiffness = 200f;
        float damping = 15f;

        while (Mathf.Abs(goalValue - currentValue) > 0.001f || Mathf.Abs(velocity) > 0.001f)
        {
            float acceleration = (stiffness * (goalValue - currentValue)) - (damping * velocity);
            velocity += acceleration * Time.deltaTime;
            currentValue += velocity * Time.deltaTime;

            target.localScale = Vector3.LerpUnclamped(squishedScale, originalScale, currentValue);
            yield return null;
        }

        target.localScale = originalScale;

        if (disableWhenFinished)
        {
            target.gameObject.SetActive(false);
        }
    }

}