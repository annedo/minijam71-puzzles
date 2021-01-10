using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public int CurrentMoney = 0;
    public int Quota = 0;

    public Text MoneyText;
    public Text QuotaText;

    public Color BelowQuotaColor;
    public Color MetQuotaColor;

    // Start is called before the first frame update
    void Start()
    {
        MoneyText.text = "$" + CurrentMoney.ToString();
        QuotaText.text = "Quota: $" + Quota.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Set text to currentMoney
        MoneyText.text = "$" + CurrentMoney.ToString();

        if (CurrentMoney < Quota)
            MoneyText.color = BelowQuotaColor;
        else
            MoneyText.color = MetQuotaColor;
    }
}