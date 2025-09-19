using Mirror;
using UnityEngine;
using System.Collections;
using static Pirata;

public class Cowboy : Personagem
{
    public enum State { Normal, Lasso, Rifle, Ulting }

    public RangedWeapon rifle, primeiraPistola, segundaPistola;
    [SyncVar] public RangedWeapon armaAtual;
    [SyncVar] public State estado;

    private Coroutine idleRoutine;

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
        else if (armaAtual == rifle && armaAtual.currentAmmo <= 0)
        {
            Debug.Log("Acabou a munição do rifle, guardando...");
            skill2.CmdEndSkill();
        }
        else if (canAttack && canReload && armaAtual.currentAmmo <= 0 && !armaAtual.reloading)
        {
            PlayReloadAnimation();
            armaAtual.Reload();
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
            anim.anim.SetTrigger("Laco");
            StartCoroutine(AnimationCheck());
        }

        if (input.Skill2Input() && canUseSkill2)
        {
            skill2.Action();
            //anim.anim.SetTrigger(estado == State.Rifle ? "EndRifle" : "StartRifle");
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
                RestartReturnToIdle();
                break;

            default:
                if (armaAtual == rifle)
                {
                    anim.anim.SetTrigger("ShootRifle");
                }
                else
                {
                    anim.anim.SetTrigger("Shoot");
                }
                RestartReturnToIdle();
                break;
        }
    }

    private void PlayReloadAnimation()
    {
        anim.anim.SetTrigger("Reload");
        RestartReturnToIdle();
    }

    IEnumerator AnimationCheck()
    {
        RestartReturnToIdle();
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
            yield return new WaitForEndOfFrame();

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|Laco"))
        {
            skill1.CmdStartSkill();
            StopIdleRoutine();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|UltD"))
        {
            segundaPistola.Action();
            StopIdleRoutine();
        }
    }

    public IEnumerator StartRifle()
    {
        estado = State.Rifle;
        RestartReturnToIdle();
        anim.anim.SetTrigger("StartRifle");

        yield return new WaitForSeconds(skill2.activationTime);

        armaAtual = rifle;
        skill2.CmdStartSkill();
        StopIdleRoutine();
        idleRoutine = StartCoroutine(ReturnToIdleRifle());
    }

    public IEnumerator EndRifle()
    {
        estado = State.Normal;
        RestartReturnToIdle();
        anim.anim.SetTrigger("EndRifle");

        yield return new WaitForSeconds(skill2.activationTime);

        armaAtual = primeiraPistola;
        StopIdleRoutine();
        idleRoutine = StartCoroutine(ReturnToIdleNormal());
    }

    // ---- Idles ----
    public IEnumerator ReturnToIdleNormal()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        anim.anim.SetTrigger("Idle");
        ResetAbilities();
    }

    public IEnumerator ReturnToIdleRifle()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        anim.anim.SetTrigger("IdleRifle");
        ResetAbilities();
    }

    private void ResetAbilities()
    {
        canAttack = true;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
        canReload = true;
    }

    private void RestartReturnToIdle()
    {
        StopIdleRoutine();
        idleRoutine = StartCoroutine(estado == State.Rifle ? ReturnToIdleRifle() : ReturnToIdleNormal());
    }

    private void StopIdleRoutine()
    {
        if (idleRoutine != null)
        {
            StopCoroutine(idleRoutine);
            idleRoutine = null;
        }
    }

    IEnumerator ShootingAnimation()
    {
        //RestartReturnToIdle();
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
            yield return new WaitForEndOfFrame();

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|AtaqueNormal") ||
            anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|ShotRifle") ||
            anim.anim.GetCurrentAnimatorStateInfo(1).IsName("RigCowboy|UltE"))
        {
            RestartReturnToIdle();
        }
    }
}
