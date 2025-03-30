using Mirror;
using UnityEngine;

public abstract class Skill : NetworkBehaviour
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

    [Command(requiresAuthority = false)]
    public virtual void CmdStartSkill(){}

    [Command(requiresAuthority = false)]
    public virtual void CmdEndSkill(){}

}
