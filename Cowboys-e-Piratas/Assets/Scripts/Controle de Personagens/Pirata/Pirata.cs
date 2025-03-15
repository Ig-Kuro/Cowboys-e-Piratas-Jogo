using UnityEngine;
using static Pirata;

public class Pirata : Personagem
{
    public bool canAttack = true;
    public enum Estado { Normal, Curando, Ultando, Atirando };  
    public Estado state;
    public GameObject jarraDeSuco;
    public Arma flintKnock;

    void Awake()
    {
        canAttack = true;
        currentHp = maxHp;
        canUseSkill1 = true;
        canUseSkill2 = true;
    }

    private void Update()
    {
        if (input.AttackInput())
        {
            armaPrincipal.Action();
        }

        if (input.Skill1Input())
        {
            if (canUseSkill1)
            {
                skill1.Action();
            }
        }

        if (input.Skill2Input())
        {
            if (canUseSkill2)
            {
                skill2.Action();
            }
        }

        if (input.UltimateInput())
        {
            if(canUlt)
            {
                ult.Action();
            }
        }
    }

}
