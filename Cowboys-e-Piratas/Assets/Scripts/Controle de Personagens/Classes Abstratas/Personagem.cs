using UnityEngine;

[RequireComponent(typeof(Movimentacao))]
public abstract class Personagem : MonoBehaviour
{
    public int hp;
    public float speed;
    public float armor;

    public bool canUseSkill1, canUseSkill2;


    public Skill skill1, skill2;
    public Arma armaPrincipal;
    public Ultimate ult;
    public InputController input;
}
