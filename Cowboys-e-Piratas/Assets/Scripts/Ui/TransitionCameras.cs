using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using Unity.VisualScripting;

public class TransitionCameras : MonoBehaviour
{
    public GameObject charactersCam, settingsCam, creditsCam, playCam, tutorialCam, exitCam, regularCam;
    public Animator macacoAnim;
    public GameObject[] allCams;

    public StartExit startExit;

    public QuitLobbyButton quitLobbyButton;
    public SettingsMenu sm;
    Animation animat;

    public void SeeCharacters()
    {
        charactersCam.SetActive(true);
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }

        CheckAnimations();
    }

    void CheckAnimations()
    {
        if (macacoAnim.GetCurrentAnimatorStateInfo(0).IsName("Armature_EstralarDedo") && macacoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            quitLobbyButton.OnHostButtonPressed();
        }

        if (macacoAnim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Pizza/FimWave") && macacoAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            LoadScena(7,0);
        }
    }

    public void GoBack()
    {
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
        creditsCam.SetActive(true);
        macacoAnim.SetTrigger("Creditos");
        sm.DeactivateMenu();

    }

    public void SeePlay()
    {
        StopAllCoroutines();
        playCam.SetActive(true);
        macacoAnim.SetTrigger("Jogar");
        sm.DeactivateMenu();
    }

    public void SeeTutorial()
    {
        tutorialCam.SetActive(true);
        macacoAnim.SetTrigger("Tutorial");
        StartCoroutine(LoadScena(5, 3));
        sm.DeactivateMenu();
    }

    public void SeeExit()
    {
        StopAllCoroutines();
        exitCam.SetActive(true);
        macacoAnim.SetTrigger("Sair");
        StartCoroutine(LoadScena(-1, 3));
        sm.DeactivateMenu();
    }

   public void SeeFeature()
    {
        tutorialCam.SetActive(true);
        macacoAnim.SetTrigger("Tutorial");
        StartCoroutine(LoadScena(3, 3));
        sm.DeactivateMenu();
    }
     public void SeeNovidades()
    {
        tutorialCam.SetActive(true);
        macacoAnim.SetTrigger("Tutorial");
        StartCoroutine(LoadScena(4, 3));
        sm.DeactivateMenu();
    }

    IEnumerator LoadScena(int v, int sec)
    {
        yield return new WaitForSeconds(sec);
        if (v >= 0)
        {
            startExit.StartGame(v);
        }
        else
        {
            Application.Quit();
        }
    }

}
