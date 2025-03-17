using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public float maxCooldown;
    public float currentCooldown;
    public bool usando = false;

    public abstract void Action();
    void Awake()
    {
        currentCooldown = maxCooldown;
    }
    public void FixedUpdate()
    {
        if (currentCooldown >= maxCooldown || usando)
        {
            return;
        }
        currentCooldown += Time.deltaTime;
    }

    public bool FinishedCooldown()
    {
        if (currentCooldown >= maxCooldown)
        {
            return true;
        }
        return false;
    }

    public abstract void StartSkill();
    public abstract void EndSkill();

}
