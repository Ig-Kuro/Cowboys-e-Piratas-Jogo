using UnityEngine;

public abstract class Ultimate : MonoBehaviour
{

    public float maxCharge;

    public float currentCharge;

    public float duration;

    public abstract void Action();
    public void ganharUlt(float amount)
    {
        currentCharge += amount;
    }

    public abstract void StartUltimate();

    public abstract void EndUltimate();

}
