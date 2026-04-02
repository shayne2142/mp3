using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    public TextMeshProUGUI moneyDisplay;
    public ParticleSystem moneyParticle;

    public float moneyCount = 0f;
    public float multiplier = 1.0f;
    public float passiveRate = 0.5f;
    public GameObject revenueTrophy;
    private bool trophyAwarded = false;

    public ParticleSystem moneyTrophyParticle;

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        moneyCount += amount * multiplier;
        if (moneyParticle != null)
        {
            moneyParticle.Stop();
            moneyParticle.Play();
        }
    }

    public void Update()
    {
        if (passiveRate > 0)
        {
            moneyCount += passiveRate * Time.deltaTime;
            UpdateUI();
            CheckTrophy();
        }
    }

    void CheckTrophy()
    {
        if (!trophyAwarded && moneyCount >= 1000000f)
        {
            trophyAwarded = true;
            revenueTrophy.SetActive(true);
            if (moneyTrophyParticle != null)
            {
                moneyTrophyParticle.Stop();
                moneyTrophyParticle.Play();
            }

            Debug.Log("Revenue Trophy Unlocked!");
        }
    }

    public void AddPassiveRate(float amount)
    {
        passiveRate += amount * multiplier;
    }

    void UpdateUI()
    {
        moneyDisplay.text = "Money: $" + moneyCount.ToString("F2");
    }
}