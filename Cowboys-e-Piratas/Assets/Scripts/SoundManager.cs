using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] cowboyLines;
    public AudioSource[] pirataLines;
    public AudioMixer mixer;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            //Esse dont destroy n funciona se for objeto filho
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
