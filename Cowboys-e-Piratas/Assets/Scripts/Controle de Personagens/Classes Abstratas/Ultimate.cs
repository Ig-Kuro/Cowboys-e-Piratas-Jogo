using UnityEngine;

public abstract class Ultimate : MonoBehaviour
{

    public float maxCharge;

    public float currentCharge;


    public abstract void Action();
    public void ganharUlt(float amount)
    {
        currentCharge += amount;
    }
}
