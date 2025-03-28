using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Cowboy : Personagem
{

    public enum state {Normal, lasso, rifle, ulting}
    public Gun rifle, primeiraPistola, segundaPistola;
    public Gun armaAtual;
    public state estado;
    public float buffer;
    float timer;
    bool attacBuffer, reloadBuffer, skill1Buffer, skill2Buffer, ultBuffer, secondaryFireBuffer;

    public void Awake()
    {
        armaAtual = primeiraPistola;
        currentHp = maxHp;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
        canAttack = true;
        canReload = true;
    }
    private void Update()
    {
        if (input.AttackInput())
        {
            if (canAttack && armaAtual.currentAmmo > 0)
            {
                armaAtual.Action();
                UIManagerCowboy.instance.AttAmmo();
            }
            else if(canAttack && canReload && armaAtual.currentAmmo == 0)
            {
                armaAtual.Reload();
                UIManagerCowboy.instance.AttAmmo();
            }
        }

        if (input.SecondaryFireInput())
        {
            if (estado != state.ulting)
            {
                return;
            }
            else
            {
                segundaPistola.Action();
                UIManagerCowboy.instance.AttAmmo();
            }
        }

        if (input.Skill1Input())
        {
            if (canUseSkill1)
            {
                skill1.Action();
                UIManagerCowboy.instance.Skill1StartCD();
            }
        }

        if (input.Skill2Input())
        {
            if(canUseSkill2)
            { 
                skill2.Action();
                UIManagerCowboy.instance.Skill2StartCD();
            }
        }

        if (input.UltimateInput())
        {
            if (canUlt)
            {
                ult.Action();
            }
        }

        if(input.ReloadInput())
        {
            if(canReload)
            {
                armaAtual.Reload();
            }
        }
    }
}
