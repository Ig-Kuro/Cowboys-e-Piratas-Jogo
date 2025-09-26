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

    [SerializeField] WeaponChanger weaponChanger;

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
        segundaPistola.Action();
        StopIdleRoutine();
    }

    private void HandleSkillInputs()
    {
        if (input.Skill1Input() && canUseSkill1)
        {
            skill1.Action();
        }

        if (input.Skill2Input() && canUseSkill2)
        {
            skill2.Action();
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
                //RestartReturnToIdle();
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

    public IEnumerator StartRifle()
    {
        estado = State.Rifle;
        
        anim.anim.SetTrigger("StartRifle");
        weaponChanger.DisableWeapon(0, 0.5f); // Desativa pistola

        yield return new WaitForSeconds(skill2.activationTime);

        armaAtual = rifle;
        skill2.CmdStartSkill();
        RestartReturnToIdle();
    }

    public IEnumerator EndRifle()
    {
        estado = State.Normal;
        RestartReturnToIdle();
        anim.anim.SetTrigger("EndRifle");
        weaponChanger.EnableWeapon(0, 1.6f); // Ativa pistola

        yield return new WaitForSeconds(skill2.activationTime);

        armaAtual = primeiraPistola;
        ResetAbilities();
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

    public void ResetAbilities()
    {
        canAttack = true;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
        canReload = true;
    }

    public void RestartReturnToIdle()
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
}
