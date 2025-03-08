using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Cowboy : Personagem
{

    public enum state {Normal, skill1, skill2, ulting}
    public Gun rifle, primeiraPistola, segundaPistola;
    public Gun armaAtual;
    public state estado;

    public void Awake()
    {
        armaAtual = primeiraPistola;
        canUseSkill1 = true;
        canUseSkill2 = true;
    }
    private void Update()
    {
        if (input.AttackInput())
        {
            if(estado == state.ulting)
            {
                primeiraPistola.Action();
                segundaPistola.Action();
            }
            else if(estado == state.skill1)
            {
                return;
            }
            else
            {
                armaAtual.Action();
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
            if(canUseSkill2)
            { 
                skill2.Action(); 
            }
        }

        if (input.UltimateInput())
        {
            ult.Action();
        }

        if(input.ReloadInput())
        {
            armaAtual.Reload();
        }
    }
}
