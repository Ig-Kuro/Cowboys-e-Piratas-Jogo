using UnityEngine;

[CreateAssetMenu]
public class SoundsSettingValues : ScriptableObject
{
	[SerializeField] private float masterVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private float musicVolume;
    [SerializeField] private float voiceVolume;


    public float ChangeMasterVolume
	{
		get { return masterVolume; }
		set { masterVolume = value; }
	}
    public float ChangeSFXVolume
    {
        get { return sfxVolume; }
        set { sfxVolume = value; }
    }
    public float ChangeMusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = value; }
    }
    public float ChangeVoiceVolume
    {
        get { return voiceVolume; }
        set { voiceVolume = value; }
    }

}
