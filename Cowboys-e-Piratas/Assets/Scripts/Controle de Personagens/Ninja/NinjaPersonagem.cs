using UnityEngine;
using System.Collections;
using Mirror;

public class NinjaPersonagem : Personagem
{
    public enum State { Normal, Melee, Trocando, Ulting }

    public RangedWeapon shuriken, kunai, ultimateSword;
    public MeleeWeapon katana;
    [SyncVar] public RangedWeapon armaAtual;
    [SyncVar] public State estado;

    void Awake()
    {
        armaAtual = shuriken;
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
        HandleSkillsInput();
    }


    private void HandleAttackInput()
    {
        if (!input.AttackInput()) return;

        if (canAttack && armaAtual.canShoot && estado != State.Ulting)
        {
            anim.anim.SetTrigger("Ataque");
            StartCoroutine(ShootingAnimation());
        }

        else if(canAttack && ultimateSword.canShoot && estado == State.Ulting)
        {
            anim.anim.SetTrigger("UltAttack");
            StartCoroutine(UltimateAttack());
        }
    }

    public void HandleSkillsInput()
    {

        if (canUseSkill1 && input.Skill1Input())
        {
            anim.anim.SetTrigger("SwitchWeapon");
            StartCoroutine(SwitchAnimation());
        }

        if (canUseSkill2 && input.Skill2Input())
        {
            
            anim.anim.SetTrigger("Melee");
            StartCoroutine(MeleeAnimation());
        }
    }



    IEnumerator ShootingAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.8)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque1"))
        {
            armaAtual.Action();
            StopCoroutine(ReturnToIdle());
        }
    }

    IEnumerator UltimateAttack()
    {
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("UltAttack"))
        {
            katana.CmdPerformAttack(transform.position, 3, transform.forward);
            ultimateSword.Action();
            StopCoroutine(ReturnToIdle());
        }
    }

    IEnumerator SwitchAnimation()
    {
        StartCoroutine(ReturnToIdle());
        skill1.Action();
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("SwitchWeapon"))
        {
            skill1.CmdStartSkill();
            StopCoroutine(ReturnToIdle());
        }
    }

    IEnumerator MeleeAnimation()
    {
        skill2.Action();
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.85)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Melee"))
        {
            katana.CmdPerformAttack(transform.position, 1, transform.forward);
            StopCoroutine(ReturnToIdle());
        }
    }

    public IEnumerator ReturnToIdle()
    {
        yield return new WaitForSecondsRealtime(3f);
        anim.anim.SetTrigger("Idle");
        canAttack = true;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
        estado = State.Normal;
        armaAtual.currentAmmo = 1;
    }
}
