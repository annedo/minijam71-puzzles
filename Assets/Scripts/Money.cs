using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public int CurrentMoney = 0;
    public int Quota = 0;

    public Text MoneyText;
    public Text QuotaText;

    public Color BelowQuotaColor;
    public Color MetQuotaColor;

    public bool QuotaVisible = true;

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

        if (SceneManager.GetActiveScene().name == "Upgrades")
            MoneyText.color = Color.black;
        else if (CurrentMoney < Quota)
            MoneyText.color = BelowQuotaColor;
        else
            MoneyText.color = MetQuotaColor;

        QuotaText.gameObject.SetActive(QuotaVisible);
    }
}