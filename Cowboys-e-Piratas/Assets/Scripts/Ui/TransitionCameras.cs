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
        //anim.SetTrigger("Idle");
        foreach (var cam in allCams)
        {
            cam.gameObject.SetActive(false);
        }
    }

    public void SeeSettings()
    {
        StopAllCoroutines();
        settingsCam.SetActive(true);
        //anim.SetTrigger("Settings");
    }

    public void SeeCredits()
    {
        StopAllCoroutines();
        charactersCam.SetActive(true);
        //anim.SetTrigger("Credits");
        StartCoroutine(LoadScena(7));

    }

    public void SeePlay()
    {
        StopAllCoroutines();
        playCam.SetActive(true);
        //anim.SetTrigger("Play");
        StartCoroutine(HostLobby());
    }

    public void SeeTutorial()
    {
        tutorialCam.SetActive(true);
        //anim.SetTrigger("Tutorial");
        StartCoroutine(LoadScena(4));
    }

    public void SeeExit()
    {
        StopAllCoroutines();
        exitCam.SetActive(true);
        //anim.SetTrigger("Exit");
        StartCoroutine(LoadScena(0));
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
