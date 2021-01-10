using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelection : MonoBehaviour
{
    public bool Selected = false;

    // Update is called once per frame
    void Update()
    {
        // Check if Selected is true or false. Set sprite accordingly
        // TODO - swap out sprite for highlighted sprite
        // https://answers.unity.com/questions/741893/how-to-change-sprite-image-on-click.html

        if (Input.GetMouseButtonDown(0))
        {            
            Selected = true;

            var gh = GameObject.Find("GridHolder");
            var gm = gh.GetComponent<GridManager>();
            gm.CheckSwap();
        }
    }
}