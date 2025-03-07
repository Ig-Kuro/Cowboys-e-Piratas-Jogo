using UnityEngine;

public abstract class Arma : MonoBehaviour
{
    [Header("Valores Universais")]
    public float attackRate;
    public float reach;
    public float damage;

    public abstract void Action();


}
