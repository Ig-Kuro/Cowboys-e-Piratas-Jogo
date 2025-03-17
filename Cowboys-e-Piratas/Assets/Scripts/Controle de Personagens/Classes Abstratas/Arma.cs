using Mirror;
using UnityEngine;

public abstract class Arma : NetworkBehaviour
{
    [Header("Valores Universais")]
    public float attackRate;
    public float reach;
    public int damage;

    public abstract void Action();


}
