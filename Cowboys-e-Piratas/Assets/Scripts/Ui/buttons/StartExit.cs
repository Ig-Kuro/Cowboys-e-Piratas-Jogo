using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartExit : MonoBehaviour
{
    [SerializeField]
    int sceneNBR;
    [SerializeField]
    Animator trans;

    [SerializeField]
    GameObject canvasTrans;
    void Start()
    {
        StartCoroutine(LevelIn());
        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void StartGame(int v)
    {
        SceneManager.LoadScene(v);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    IEnumerator LevelIn()
    {
        yield return new WaitForSeconds(1.3f);
        //canvasTrans.SetActive(true);
    }
    public void LevelOut()
    {
        StartCoroutine("TransitionMenu");
    }
    public void Resume()
    {
        Time.timeScale=1;
    }
    IEnumerator TransitionMenu()
    {
        trans.SetTrigger("IndoVindo");
        //canvasTrans.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        StartGame(sceneNBR);
    }
}
