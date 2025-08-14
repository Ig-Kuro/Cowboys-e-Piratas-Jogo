using Mirror;
using UnityEngine;

public abstract class BaseWeapon : NetworkBehaviour
{
    [Header("Valores Universais")]
    public float attackRate;
    public float reach;
    public int damage;
    public bool useAmmo = false;
    public Ultimate ultimate;
    public Personagem player;

    public abstract void Action();
}
