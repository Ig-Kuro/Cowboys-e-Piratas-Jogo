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
    public void ganharUlt(float amount)
    {
        if(usando) 
        {
            return;
        }
        currentCharge += amount;
    }

    public bool Carregado()
    {
        if(currentCharge >= maxCharge)
        {
            return true;
        }
        return false;
    }
    public void LevelUP()
    {
        upgradeLV+=1;
        maxCharge= maxCharge / ((10/100*(upgradeLV)));
    }

    public virtual void CmdStartUltimate(){}

    public virtual void CmdEndUltimate(){}

    public virtual void CmdCancelUltimate(){}

}
