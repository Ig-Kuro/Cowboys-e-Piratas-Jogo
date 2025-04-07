using UnityEngine;
using UnityEngine.SceneManagement;

public class StartExit : MonoBehaviour
{
    [SerializeField]
    int sceneNBR;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(sceneNBR);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale=1;
    }
}
