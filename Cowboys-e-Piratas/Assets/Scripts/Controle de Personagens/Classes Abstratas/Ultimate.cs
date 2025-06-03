using UnityEngine;
using Mirror;

public abstract class Ultimate : NetworkBehaviour
{
    public AudioSource audioStart, audioEnd;
    public float maxCharge;

    public float currentCharge;

    public float duration;
    public bool usando;
    public int upgradeLV=0;

    public Sprite icon;
    
    public abstract void Action();
    public void AddUltPoints(float amount)
    {
        if(usando)
        {
            return;
        }
        currentCharge += amount;
        if(currentCharge >= maxCharge)
        {
            currentCharge = maxCharge;
        }
    }

    public bool UltReady()
    {
        if(currentCharge >= maxCharge)
        {
            return true;
        }
        return false;
    }
    public void UltLevelUp()
    {
        upgradeLV+=1;
        maxCharge/= 10/100*upgradeLV;
    }

    public virtual void CmdStartUltimate(){}

    public virtual void CmdEndUltimate(){}

    public virtual void CmdCancelUltimate(){}

}
