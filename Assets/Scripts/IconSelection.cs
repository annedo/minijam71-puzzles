using UnityEngine;

public class IconSelection : MonoBehaviour
{
    public Sprite spriteUnselected;
    public Sprite spriteSelected;

    public bool Selected = false;

    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = spriteUnselected;
    }

    public void OnMouseDown()
    {        
        if (Selected)
        {
            Selected = false;
            return;
        }

        Selected = true;

        var gh = GameObject.Find("GridHolder");
        var gm = gh.GetComponent<GridManager>();
        gm.CheckSwap();        
    }

    public void Update()
    {
        // This is probably inefficient
        if (Selected)
            _spriteRenderer.sprite = spriteSelected;
        else
            _spriteRenderer.sprite = spriteUnselected;
    }
}