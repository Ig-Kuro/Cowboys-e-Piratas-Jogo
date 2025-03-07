using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float maxCooldown;
    public float currentCooldown;


    public abstract void Action();
}
