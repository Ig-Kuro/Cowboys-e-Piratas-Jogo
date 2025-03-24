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

    private void Update()
    {
        if(!isLocalPlayer) return;
        if (input.AttackInput())
        {
            if (canAttack && state != Estado.Ultando)
            {
                armaPrincipal.Action();
            }
            else if (state == Estado.Ultando)
            {
                ult.StartUltimate();
            }
                
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

        if(input.SecondaryFireInput())
        {
            if (state == Estado.Ultando)
            {
                ult.CancelUltimate();
            }
        }
    }

}
