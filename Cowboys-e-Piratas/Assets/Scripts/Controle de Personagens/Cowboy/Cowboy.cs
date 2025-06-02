using Mirror;
using UnityEngine;

public class Cowboy : Personagem
{
    public enum state {Normal, lasso, rifle, ulting}
    public Gun rifle, primeiraPistola, segundaPistola;
    [SyncVar]
    public Gun armaAtual;
    [SyncVar]
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
        //UIManagerCowboy.instance.AttAmmo(armaAtual);
    }
    public override void Start()
    {
        if (isLocalPlayer)
        {
            clippingMesh.SetActive(false);
            base.Start();
        }
    }

    private void Update()
    {
        if(!isLocalPlayer) return;
        if (input.AttackInput())
        {
            if (canAttack && armaAtual.currentAmmo > 0 && armaAtual.canShoot && !armaAtual.reloading)
            {
                armaAtual.Action();
                if (armaAtual == rifle)
                {
                    anim.anim.SetTrigger("ShootRifle");

                }
                else if(armaAtual == primeiraPistola && estado != state.ulting)
                {
                    anim.anim.SetTrigger("Shoot");

                }
                else if(estado == state.ulting)
                {
                    anim.anim.SetTrigger("ShootE");
                }
            }
            else if(canAttack && canReload && armaAtual.currentAmmo == 0 && !armaAtual.reloading)
            {
                //armaAtual.emptyClipNoise.Play();
                anim.anim.SetTrigger("Reload");
                armaAtual.Reload();

            }
            else if(armaAtual == rifle && armaAtual.currentAmmo <= 0)
            {
                skill2.Action();
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
                anim.anim.SetTrigger("ShootD");
                //UIManagerCowboy.instance.AttAmmo(armaAtual);
            }
        }

        if (input.Skill1Input())
        {
            if (canUseSkill1)
            {
                skill1.Action();;
                //UIManagerCowboy.instance.Skill1StartCD();
                
            }
        }

        if (input.Skill2Input())
        {
            if(canUseSkill2)
            { 
                skill2.Action();
                //UIManagerCowboy.instance.Skill2StartCD();
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
            if(canReload && !armaAtual.reloading)
            {
                armaAtual.Reload();
                if(armaAtual == rifle)
                {

                }
                else
                {
                    anim.anim.SetTrigger("Reload");
                }
            }
        }
    }
}
