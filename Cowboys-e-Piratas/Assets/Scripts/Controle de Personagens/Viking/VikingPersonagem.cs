using UnityEngine;
using System.Collections;

public class VikingPersonagem : Personagem
{
    public enum Estado { Normal, Gritando, Ultando, Porradando }
    public Estado state;
    public MeleeWeapon axe;
    public RangedWeapon crystalWave;
    public int damgeMultiplier = 1;
    public bool drainHp = false;

    void Awake()
    {
        canAttack = canUseSkill1 = canUseSkill2 = canUlt = true;
        currentHp = maxHp;
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

        if (canAttack && state != Estado.Porradando && state != Estado.Gritando
            && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque"))
        {
            anim.anim.SetTrigger("Ataque");
            StartCoroutine(AttackAnimation());
        }
    }

    public void HandleSkillsInput()
    {

        if (canUseSkill1 && input.Skill1Input() && skill1.FinishedCooldown())
        {
            anim.anim.SetTrigger("Grito");
            skill1.Action();
            StartCoroutine(GritoAnimation());
        }

        if (canUseSkill2 && input.Skill2Input() && skill2.FinishedCooldown())
        {
            skill2.Action();
            anim.anim.SetTrigger("Porradao");
            StartCoroutine(PorradaoAnimation());
        }

        if (canUlt && input.UltimateInput() && ult.currentCharge >= ult.maxCharge)
        {
            ult.Action();
            anim.anim.SetTrigger("Ult");
            StartCoroutine(UltimateStart());
        }
    }

    IEnumerator AttackAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.85)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque"))
        {
            axe.CmdPerformAttack(transform.position, 1 * damgeMultiplier, transform.forward);
            StopCoroutine(ReturnToIdle());
        }
    }


    IEnumerator GritoAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.6f)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Stun"))
        {
            skill1.CmdStartSkill();
            skill1.CmdEndSkill();
            StopCoroutine(ReturnToIdle());
        }
    }

    IEnumerator PorradaoAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.85)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Porradao"))
        {
            axe.CmdPerformAttack(transform.position, 1 * damgeMultiplier, transform.forward);
            crystalWave.Action();
            skill2.CmdEndSkill();
            StopCoroutine(ReturnToIdle());
        }
    }

    IEnumerator UltimateStart()
    {
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("StartUlt"))
        {
            ult.CmdStartUltimate();
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
        state = Estado.Normal;
    }
}
