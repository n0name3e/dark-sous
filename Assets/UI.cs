using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public GameObject restartButton;
    public GameObject quitButton;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
     
    // or hide
    public void DisplayGameManagingButtons()
    {
        restartButton.SetActive(!restartButton.activeInHierarchy);
        quitButton.SetActive(!quitButton.activeInHierarchy);
    }
    public void RestartGame()
    {
        LevelUpdateManager.instance.UpdateLevel(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
