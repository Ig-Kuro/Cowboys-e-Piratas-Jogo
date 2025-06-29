using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private SoundsSettingValues soundValues;
    [SerializeField] private CameraSettingsValues cameraValues;
    public static SettingsMenu instance;
    public CameraControl cc;
    public Slider masterVolumeSlider, SFXVolumeSlider, musicVolumeSlider, voiceVolumeSlider, senseXSlider, senseYSlider;
    public GameObject settingPanel;
    bool isInverted = false;

    private void Start()
    {
        instance = this;
        ChangeMasterVolume(soundValues.ChangeMasterVolume);
        masterVolumeSlider.value = soundValues.ChangeMasterVolume;

        ChangeSFXVolume(soundValues.ChangeSFXVolume);
        SFXVolumeSlider.value = soundValues.ChangeSFXVolume;

        ChangeMusicVolume(soundValues.ChangeMusicVolume);
        musicVolumeSlider.value = soundValues.ChangeMusicVolume;

        ChangeVoiceVolume(soundValues.ChangeVoiceVolume);
        voiceVolumeSlider.value = soundValues.ChangeVoiceVolume;

        senseXSlider.value = cameraValues.ChangeSenseX;
        ChangeSenseX(cameraValues.ChangeSenseX);

        senseYSlider.value = cameraValues.ChangeSenseY;
        ChangeSenseY(cameraValues.ChangeSenseY);

        isInverted = cameraValues.IsInverted;
         if (isInverted && cc!= null)
         {
                cc.invertControl = -1;
         }
         else if(!isInverted && cc!= null)
         {
                cc.invertControl = 1;
         }
        
    }

    public void ActivateMenu()
    {
        settingPanel.SetActive(true);
    }

    public void DeactivateMenu()
    {
        settingPanel.SetActive(false);
    }

    public void ChangeMasterVolume(float v)
    {
        SoundManager.instance.mixer.SetFloat("MasterVolume", v);
        soundValues.ChangeMasterVolume = v;
    }

    public void ChangeSFXVolume(float v)
    {
        SoundManager.instance.mixer.SetFloat("SFXVolume", v);
        soundValues.ChangeSFXVolume = v;
    }

    public void ChangeMusicVolume(float v)
    {
        SoundManager.instance.mixer.SetFloat("MusicVolume", v);
        soundValues.ChangeMusicVolume = v;
    }

    public void ChangeVoiceVolume(float v)
    {
        SoundManager.instance.mixer.SetFloat("VoiceVolume", v);
        soundValues.ChangeVoiceVolume = v;
    }


    public void InvertSensibility()
    {
        if(isInverted)
        {
            cameraValues.IsInverted = false;
            isInverted = false;
            if (cc != null)
            {
                cc.invertControl = 1;
            }
        }
        else
        {
            cameraValues.IsInverted = true;
            isInverted = true;
            if (cc != null)
            {
                cc.invertControl = -1;
            }
        }
    }


    public void ChangeSenseX(float v)
    {
        if (cc != null)
        {
            cc.sensitivityX = v;
        }   
        cameraValues.ChangeSenseX = v;
    }

    public void ChangeSenseY(float v)
    {

        if (cc != null)
        {
            cc.sensitivityY = v;
        }
        cameraValues.ChangeSenseY = v;
    }
}
