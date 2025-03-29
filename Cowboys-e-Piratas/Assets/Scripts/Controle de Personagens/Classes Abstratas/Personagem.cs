using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Movimentacao))]
public abstract class Personagem : NetworkBehaviour
{
    public int currentHp, maxHp;
    public float speed;
    public float armor;

    public bool canUseSkill1, canUseSkill2, canUlt, canAttack, canReload;


    public Skill skill1, skill2;
    public Arma armaPrincipal;
    public Ultimate ult;
    public InputController input;
    public List<Arma> weapons;
    public void TomarDano(int dano)
    {
        currentHp -= dano;
        Debug.Log("ai");
    }

    [ClientRpc]
    public virtual void RpcSetGunState(int gunIndex, bool active){
        if (gunIndex >= 0 && gunIndex < weapons.Count)
        {
            weapons[gunIndex].gameObject.SetActive(active);
        }
    }
}
