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
    }
    private void Update()
    {
        if (input.AttackInput())
        {
                armaAtual.Action();
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
            if (canUlt)
            {
                ult.Action();
            }
        }

        if(input.ReloadInput())
        {
            armaAtual.Reload();
        }
    }
}
