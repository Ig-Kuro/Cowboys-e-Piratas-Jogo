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

    private void Start()
    {
        if (isLocalPlayer)
        {
            clippingMesh.SetActive(false);
        }
    }

    private void Update()
    {
        if(!isLocalPlayer) return;
        anim.anim.SetInteger("Attacking", weapon.currentCombo);
        if (input.AttackInput())
        {
            if (canAttack && state != Estado.Ultando)
            {
                weapon.WeaponSwing();
            }
            else if (state == Estado.Ultando)
            {
                ult.CmdStartUltimate();
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
                }
            }
        }

        if (input.UltimateInput())
        {
            if(canUlt)
            {
                ult.Action();
            }
        }

        if(input.SecondaryFireInput())
        {
            if (state == Estado.Ultando)
            {
                ult.CmdCancelUltimate();
            }
        }
    }
    

}
