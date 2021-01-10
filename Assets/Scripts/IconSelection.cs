using UnityEngine;

public class IconSelection : MonoBehaviour
{
    public bool Selected = false;

    public void OnMouseDown()
    {
        // Check if Selected is true or false. Set sprite accordingly
        // TODO - swap out sprite for highlighted sprite
        // https://answers.unity.com/questions/741893/how-to-change-sprite-image-on-click.html

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
}