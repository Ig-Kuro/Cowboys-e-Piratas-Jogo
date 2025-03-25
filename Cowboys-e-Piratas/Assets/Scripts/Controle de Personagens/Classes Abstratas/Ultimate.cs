using UnityEngine;

public abstract class Ultimate : MonoBehaviour
{

    public float maxCharge;

    public float currentCharge;

    public float duration;
    public bool usando;
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

    public abstract void StartUltimate();

    public abstract void EndUltimate();
    public abstract void CancelUltimate();

}
