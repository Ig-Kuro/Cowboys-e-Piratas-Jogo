using Mirror;
using UnityEngine;

public abstract class Skill : NetworkBehaviour
{
    public AudioSource audioStart, audioEnd;
    public float maxCooldown;
    public float currentCooldown;
    public bool usando = false;

    public CooldownIcon ci;

    public Sprite icon;

    public int upgradeLV=0;

    public abstract void Action();
    void Awake()
    {
        currentCooldown = maxCooldown;
    }
    public void FixedUpdate()
    {
        if (currentCooldown >= maxCooldown || usando)
        {
            if(ci != null)
            {
                ci.inCooldown = false;
            }
            return;
        }
        currentCooldown += Time.deltaTime;
        if(ci != null)
        {
            ci.inCooldown = true;
        }
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
