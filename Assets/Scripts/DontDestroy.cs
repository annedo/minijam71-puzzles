using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        var objs = FindObjectsOfType<DontDestroy>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}