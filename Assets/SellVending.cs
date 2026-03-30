using UnityEngine;
using System.Collections;

public class SellVending : MonoBehaviour
{
    public float vendTime = 3.0f;
    public float vendValue = 3.0f;
    public float vendCount = 1;

    private MoneyCounter moneyManager;
    private VendingCount vendingCount;

    void Start()
    {
        moneyManager = Object.FindFirstObjectByType<MoneyCounter>();
        vendingCount = Object.FindFirstObjectByType<VendingCount>();
    }

    public void ActivateMachine()
    {
        StartCoroutine(VendingLoop());
    }

    IEnumerator VendingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(vendTime);
            SellItem();
        }
    }

    void SellItem()
    {
        moneyManager.AddMoney(vendValue);
        vendingCount.RegisterSale(vendCount);
    }

    public void multiplyValue(float multiplier)
    {
        vendValue *= multiplier;
    }
}