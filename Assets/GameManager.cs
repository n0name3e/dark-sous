using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isPaused = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            UI.instance.DisplayGameManagingButtons();
        }
    }
    private void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = System.Convert.ToInt32(!isPaused);
    }
}
