using UnityEngine;
using static Pirata;
using Mirror;

public class Pirata : Personagem
{ 
    public enum Estado { Normal, Curando, Ultando, Atirando };  
    public Estado state;
    public GameObject jarraDeSuco, polvoSummon;
    public Arma flintKnock;
    public MeleeWeapon weapon;
    public Animator animator;
    public float buffer;
    float timer;
    bool attacBuffer, skill1Buffer, skill2Buffer, ultBuffer;

    void Awake()
    {
        canAttack = true;
        currentHp = maxHp;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
    }

    private void Update()
    {
        if(!isLocalPlayer) return;
        if (input.AttackInput())
        {
            if (canAttack && state != Estado.Ultando)
            {
                armaPrincipal.Action();
                if(weapon.canAttack)
                {
                    animator.SetTrigger("Attack");
                }
            }
            else if (state == Estado.Ultando)
            {
                ult.CmdStartUltimate();
                animator.SetBool("Ultando", false);

            }
                
        }

        if (input.Skill1Input())
        {
            if (canUseSkill1)
            {
                if(skill1.FinishedCooldown())
                {
                    skill1.Action();
                    //UIManagerPirata.instance.Skill1StartCD();
                    animator.SetTrigger("Cura");
                }
            }
        }

        if (input.Skill2Input())
        {
            if (canUseSkill2)
            {
                if (skill2.FinishedCooldown())
                {
                    skill2.Action();
                    //UIManagerPirata.instance.Skill2StartCD();
                    animator.SetTrigger("Shoot");
                }
            }
        }

        if (input.UltimateInput())
        {
            if(canUlt)
            {
                ult.Action();
                if(ult.usando)
                {
                    Debug.Log("DeuCerto");
                    animator.SetBool("Ultando", true);

                }
            }
        }

        if(input.SecondaryFireInput())
        {
            if (state == Estado.Ultando)
            {
                ult.CmdCancelUltimate();
                animator.SetBool("Ultando", false);
            }
        }
    }

}
