using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class TransitionCameras : MonoBehaviour
{
    public GameObject charactersCam, settingsCam, creditsCam, playCam, tutorialCam, exitCam, regularCam;
    public Animator macacoAnim;
    public GameObject[] allCams;

    public StartExit startExit;

    public QuitLobbyButton quitLobbyButton;
    public SettingsMenu sm;

    public void SeeCharacters()
    {
        charactersCam.SetActive(true);
        //anim.SetTrigger("Characters");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        StopAllCoroutines();
        macacoAnim.SetTrigger("Voltar");
        foreach (var cam in allCams)
        {
            cam.gameObject.SetActive(false);
        }
        sm.DeactivateMenu();
    }

    public void SeeSettings()
    {
        StopAllCoroutines();
        settingsCam.SetActive(true);
        macacoAnim.SetTrigger("Configs");
        sm.ActivateMenu();
    }

    public void SeeCredits()
    {
        StopAllCoroutines();
        creditsCam.SetActive(true);
        macacoAnim.SetTrigger("Creditos");
        StartCoroutine(LoadScena(7));
        sm.DeactivateMenu();

    }

    public void SeePlay()
    {
        StopAllCoroutines();
        playCam.SetActive(true);
        macacoAnim.SetTrigger("Jogar");
        StartCoroutine(HostLobby());
        sm.DeactivateMenu();
    }

    public void SeeTutorial()
    {
        tutorialCam.SetActive(true);
        macacoAnim.SetTrigger("Tutorial");
        StartCoroutine(LoadScena(4));
        sm.DeactivateMenu();
    }

    public void SeeExit()
    {
        StopAllCoroutines();
        exitCam.SetActive(true);
        macacoAnim.SetTrigger("Sair");
        StartCoroutine(LoadScena(0));
        sm.DeactivateMenu();
    }


    IEnumerator LoadScena(int v)
    {
        yield return new WaitForSeconds(3f);
        startExit.StartGame(v);
    }



    IEnumerator HostLobby()
    {
        yield return new WaitForSeconds(2f);
        quitLobbyButton.OnHostButtonPressed();
    }

}
