using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{
    public AudioSource musicCowboy, musicJapan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(musicCowboy.isPlaying)
            {
                musicCowboy.Pause();
                musicJapan.Play();
            }
            else if(musicJapan.isPlaying)
            {
                musicJapan.Pause();
                musicCowboy.Play();
            }
        }
    }
}
