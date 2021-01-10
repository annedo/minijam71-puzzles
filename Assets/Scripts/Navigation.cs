using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void ChangeScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        
        switch (currentScene.name)
        {
            case "MainMenu":
                SceneManager.LoadScene("MainGame");
                break;
            case "Gameover":
                SceneManager.LoadScene("MainMenu");
                break;
            case "Upgrades":
                SceneManager.LoadScene("MainGame");
                break;
            default:
                break;
        }
    }
}