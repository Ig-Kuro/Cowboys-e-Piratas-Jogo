using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void TomarDano(int dano)
    {
        
        currentHp -= dano;
        if(currentHp<=0)
        {
            SceneManager.LoadScene(2);
        }
        Debug.Log("ai");
    }
}
