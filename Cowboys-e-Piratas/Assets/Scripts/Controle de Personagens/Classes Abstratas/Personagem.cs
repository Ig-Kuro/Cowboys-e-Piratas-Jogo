using UnityEngine;

[RequireComponent(typeof(Movimentacao))]
public abstract class Personagem : MonoBehaviour
{
    public int currentHp, maxHp;
    public float speed;
    public float armor;

    public bool canUseSkill1, canUseSkill2, canUlt, canAttack, canReload;


    public Skill skill1, skill2;
    public Arma armaPrincipal;
    public Ultimate ult;
    public InputController input;
    public void TomarDano(int dano)
    {
        currentHp -= dano;
        Debug.Log("ai");
    }
}
