using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public Sprite spriteTier1;
    public Sprite spriteTier2;
    public Sprite spriteTier3;

    public TierTracker.TierTypes _tierType;
    public Text _text;

    private SpriteRenderer _spriteRenderer;
    private Money _money;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = spriteTier1;

        _money = FindObjectOfType<Money>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (TierTracker.CurrentTier[_tierType])
        {
            case 1:
                _spriteRenderer.sprite = spriteTier1;
                break;
            case 2:
                _spriteRenderer.sprite = spriteTier2;
                break;
            case 3:
                _spriteRenderer.sprite = spriteTier3;
                break;
        }

        _text.text = $"{_tierType.ToString().ToUpper()}\nL{TierTracker.CurrentTier[_tierType]}";

        if (TierTracker.CurrentTier[_tierType] < 3)
        {
            _text.text += $"\nUpgrade: ${TierTracker.TierCosts[_tierType][TierTracker.CurrentTier[_tierType]].TierUpgradeCost}";
            _text.fontSize = 20;
        }
        else
            _text.fontSize = 30;
    }

    public void OnMouseDown()
    {
        // Money check
        if (TierTracker.CurrentTier[_tierType] < 3)
        {
            var currentTier = TierTracker.CurrentTier[_tierType];
            var upgradeCost = TierTracker.TierCosts[_tierType][currentTier].TierUpgradeCost;
            if (_money.CurrentMoney - upgradeCost >= 0)
            {
                TierTracker.CurrentTier[_tierType]++;
                _money.CurrentMoney -= upgradeCost;
            }
        }
    }
}