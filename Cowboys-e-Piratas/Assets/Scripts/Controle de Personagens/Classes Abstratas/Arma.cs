using UnityEngine;

public abstract class Arma : MonoBehaviour
{
    public float attackRate;
    public float reach;
    public float damage;
    public abstract void Action();

}
