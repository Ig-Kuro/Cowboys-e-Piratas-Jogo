using UnityEngine;
using System.Collections;
using static UnityEngine.Analytics.IAnalytic;
using static Pirata;

public class Pirata : Personagem
{
    public enum Estado { Normal, Curando, Ultando, Atirando }
    public Estado state;

    public GameObject jarraDeSuco, polvoSummon;
    public BaseWeapon flintKnock;
    public PlayerMeleeWeapon weapon;
    public float buffer;

    void Start()
    {
        if (isLocalPlayer)
            clippingMesh.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleAttackInput();
        HandleSkillInputs();
        HandleUltimateInput();
        HandleCancelUltimate();
    }

    private void HandleAttackInput()
    {
        if (!input.AttackInput()) return;

        if (state == Estado.Ultando)
        {
            ult.CmdStartUltimate();
            return;
        }

        if (canAttack)
        {
            weapon.TryAttack(); // nova função que centraliza a lógica de combo
        }
    }


    private void HandleSkillInputs()
    {
        if (input.Skill1Input() && canUseSkill1 && skill1.FinishedCooldown())
        {
            anim.Skill1Pirata();
            StartCoroutine(DrinkingAnimation());
            //UIManagerPirata.instance.Skill1StartCD();
        }

        if (input.Skill2Input() && canUseSkill2 && skill2.FinishedCooldown())
        {
            anim.Skill2Pirata();
            //UIManagerPirata.instance.Skill2StartCD();
            StartCoroutine(ShootingAnimation());
        }
    }

    private void HandleUltimateInput()
    {
        if (input.UltimateInput() && canUlt)
        {
            ult.Action();
        }
    }

    private void HandleCancelUltimate()
    {
        if (input.SecondaryFireInput() && state == Estado.Ultando)
        {
            ult.CmdCancelUltimate();
        }
    }

    IEnumerator ShootingAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9 && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("ossos pirata|AtirandoPistola/Bra�os"))
        {
            yield return new WaitForEndOfFrame();
        }
        skill2.Action();
        StopCoroutine(ReturnToIdle());
    }


    IEnumerator DrinkingAnimation()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9 && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("ossos pirata|Suco/Pegando"))
        {
            yield return new WaitForEndOfFrame();
        }
        skill1.Action();
        StartCoroutine(StopDrinking());
        StopCoroutine(ReturnToIdle());
    }

    IEnumerator StopDrinking()
    {
        StartCoroutine(ReturnToIdle());
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9 && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("ossos pirata|Suco/Guardando"))
        {
            yield return new WaitForEndOfFrame();
        }
        skill1.CmdEndSkill();
        StopCoroutine(ReturnToIdle());
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1)
        {
            anim.anim.SetTrigger("Idle");
        }

        canAttack = true;
        canUseSkill1 = true;
        canUseSkill2 = true;
        canUlt = true;
        state = Estado.Normal;

    }
}
