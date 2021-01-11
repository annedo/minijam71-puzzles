using UnityEngine;
using UnityEngine.UI;

public class Moves : MonoBehaviour
{
    public int MovesRemaining;
    public Text MovesText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovesText.text = MovesRemaining.ToString();
    }
}