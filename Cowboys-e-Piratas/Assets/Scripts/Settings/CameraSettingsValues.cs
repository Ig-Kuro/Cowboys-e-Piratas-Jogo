using UnityEngine;


[CreateAssetMenu]
public class CameraSettingsValues : ScriptableObject
{
    [SerializeField] private float senseX;
    [SerializeField] private float senseY;
    [SerializeField] private bool inverted = false;


    public float ChangeSenseX
    {
        get { return senseX; }
        set { senseX = value; }
    }

    public float ChangeSenseY
    {
        get { return senseY; }
        set { senseY = value; }
    }

    public bool IsInverted
    {
        get { return inverted; }
        set { inverted = value; }
    }
}
