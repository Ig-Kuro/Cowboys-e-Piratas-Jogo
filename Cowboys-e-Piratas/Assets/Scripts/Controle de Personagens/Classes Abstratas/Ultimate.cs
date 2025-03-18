using UnityEngine;

public abstract class Ultimate : MonoBehaviour
{

    public float maxCharge;

    public float currentCharge;

    public float duration;
    bool usando;
    public abstract void Action();
    public void ganharUlt(float amount)
    {
        if(usando) 
        {
            return;
        }
        currentCharge += amount;
    }

    public abstract void StartUltimate();

    public abstract void EndUltimate();

}
