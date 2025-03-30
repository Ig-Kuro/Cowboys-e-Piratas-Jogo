using UnityEngine;
using UnityEngine.SceneManagement;

public class StartExit : MonoBehaviour
{
    [SerializeField]
    int sceneNBR;
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
