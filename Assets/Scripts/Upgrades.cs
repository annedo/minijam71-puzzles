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

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = spriteTier1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (TierTracker.Tiers[_tierType])
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

        _text.text = $"{_tierType.ToString().ToUpper()}\nL{TierTracker.Tiers[_tierType]}";
    }

    public void OnMouseDown()
    {
        // Money check
        if (TierTracker.Tiers[_tierType] < 3)
            TierTracker.Tiers[_tierType]++;
        // TODO - Perform upgrade if money check success

        TierTracker.Tiers[_tierType] = TierTracker.Tiers[_tierType];
    }
}