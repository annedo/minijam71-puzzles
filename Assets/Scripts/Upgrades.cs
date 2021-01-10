using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public Sprite spriteTier1;
    public Sprite spriteTier2;
    public Sprite spriteTier3;

    public int CurrentTier = 1;

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
        switch (CurrentTier)
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
    }

    public void OnMouseDown()
    {
        // Money check
        if (CurrentTier < 3)
            CurrentTier++;
        // Perform upgrade if money check success
    }
}