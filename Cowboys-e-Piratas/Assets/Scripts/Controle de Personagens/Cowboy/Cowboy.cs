using Mirror;
using UnityEngine;
using System.Collections;


public class Cowboy : Personagem
{
    public enum State { Normal, Lasso, Rifle, Ulting }

    public RangedWeapon rifle, primeiraPistola, segundaPistola;
    [SyncVar] public RangedWeapon armaAtual;
    [SyncVar] public State estado;

    public float buffer;
    private float timer;

    void Awake()
    {
        armaAtual = primeiraPistola;
        currentHp = maxHp;
        canUseSkill1 = canUseSkill2 = canUlt = canAttack = canReload = true;
    }

    void Start()
    {
        if (isLocalPlayer)
            clippingMesh.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleAttackInput();
        HandleSecondaryFireInput();
        HandleSkillInputs();
        HandleUltimateInput();
        HandleReloadInput();
    }

    private void HandleAttackInput()
    {
        if (!input.AttackInput()) return;

        if (canAttack && armaAtual.canShoot && armaAtual.currentAmmo > 0 && !armaAtual.reloading)
        {
            armaAtual.Action();
            PlayShootAnimation();
        }
        else if (canAttack && canReload && armaAtual.currentAmmo == 0 && !armaAtual.reloading)
        {
            PlayReloadAnimation();
            armaAtual.Reload();
        }
        else if (armaAtual == rifle && armaAtual.currentAmmo <= 0)
        {
            skill2.Action();
        }
    }

    private void HandleSecondaryFireInput()
    {
        if (!input.SecondaryFireInput() || estado != State.Ulting) return;
       
        anim.anim.SetTrigger("ShootD");
        StartCoroutine(AnimationCheck());
    }

    private void HandleSkillInputs()
    {
        if (input.Skill1Input() && canUseSkill1)
        {
            skill1.Action();
            StopAllCoroutines();

            anim.anim.SetTrigger("Laco");

            StartCoroutine(AnimationCheck());
            //UIManagerCowboy.instance.Skill1StartCD();
        }

        if (input.Skill2Input() && canUseSkill2)
        {
            skill2.Action();
            StopAllCoroutines();

            anim.anim.SetTrigger("StartRifle");

            StartCoroutine(StartRifle());
            //UIManagerCowboy.instance.Skill2StartCD();
        }
    }

    private void HandleUltimateInput()
    {
        if (input.UltimateInput() && canUlt)
        {
            ult.Action();
        }
    }

    private void HandleReloadInput()
    {
        if (!input.ReloadInput() || !canReload || armaAtual.reloading) return;

        armaAtual.Reload();

        if (armaAtual != rifle)
            PlayReloadAnimation();
    }

    private void PlayShootAnimation()
    {
        switch (estado)
        {
            case State.Ulting:
                anim.anim.SetTrigger("ShootE");
                StartCoroutine(ShootingAnimation());
                break;
            default:
                if (armaAtual == rifle)
                {
                    anim.anim.SetTrigger("ShootRifle");
                    StartCoroutine(ShootingAnimation());
                }
                else
                    anim.anim.SetTrigger("Shoot");
                    StartCoroutine(ShootingAnimation());
                break;
        }
    }

    private void PlayReloadAnimation()
    {
        anim.anim.SetTrigger("Reload");
    }


    IEnumerator AnimationCheck()
    {
        while(anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        } 
        //Skills
        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|Laco"))
        {
            skill1.CmdStartSkill();
        }
        //

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|UltD"))
        {
            segundaPistola.Action();
        }
    }


    public IEnumerator StartRifle()
    {
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        skill2.CmdStartSkill();
    }


    public IEnumerator EndRifle()
    {
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|GetRifle"))
        {
            skill2.CmdStartSkill();
        }

        skill2.CmdEndSkill();

    }



    IEnumerator ShootingAnimation()
    {
        while(anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        armaAtual.Action();
    }
}
