using Mirror;
using UnityEngine;

public abstract class Skill : NetworkBehaviour
{
    public AudioSource audioStart, audioEnd;
    public float maxCooldown;
    public float currentCooldown;
    public bool usando = false;

    public Sprite icon;

    public int upgradeLV=0;
    public CooldownIcon ci;

    public abstract void Action();
    public override void OnStartClient()
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
    public void LevelUP()
    {
        upgradeLV+=1;
        maxCooldown= maxCooldown / ((10/100*(upgradeLV)));
    }

    public virtual void CmdStartSkill(){}

    public virtual void CmdEndSkill(){}

}
