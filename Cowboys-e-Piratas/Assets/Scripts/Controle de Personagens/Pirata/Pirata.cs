using UnityEngine;
using System.Collections;
using static UnityEngine.Analytics.IAnalytic;

public class Pirata : Personagem
{
    public enum Estado { Normal, Curando, Ultando, Atirando }
    public Estado state;

    public GameObject jarraDeSuco, polvoSummon;
    public BaseWeapon flintKnock;
    public MeleeWeapon weapon;
    public float buffer;
    private float timer;

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
        HandleSkillInputs();
        //HandleUltimateInput();
        HandleCancelUltimate();
    }

    private void HandleAttackInput()
    {
        if (!input.AttackInput()) return;

        if (canAttack && state != Estado.Ultando)
        {
            weapon.WeaponSwingPirata();
            StartCoroutine(AttackAnimation());  
        }
        else if (state == Estado.Ultando)
        {
            ult.CmdStartUltimate();
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
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.9 && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("ossos pirata|AtirandoPistola/Braços"))
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


    IEnumerator AttackAnimation()
    {
        StartCoroutine(ReturnToIdle());

        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 0.6)
            {
                yield return new WaitForEndOfFrame();
            }

        //Primeiro Ataque
        if(anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque1"))
        {
            weapon.CmdPerformAttack(weapon.transform.position, weapon.currentCombo, weapon.transform.right);
            StopCoroutine(ReturnToIdle());
            StopCoroutine(AttackAnimation());
        }

        //Segundo Ataque
        else if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque2"))
        {
            weapon.CmdPerformAttack(weapon.transform.position, weapon.currentCombo, -weapon.transform.right);
            StopCoroutine(ReturnToIdle());
            StopCoroutine(AttackAnimation());
        }

        //Terceiro Ataque
        else if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque3"))
        {
            weapon.CmdPerformAttack(weapon.transform.position, weapon.currentCombo, weapon.transform.forward);
            StopCoroutine(ReturnToIdle());
        }

        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        if (anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque3"))
        {
            weapon.CmdPerformAttack(weapon.transform.position, weapon.currentCombo, weapon.transform.forward);
            StopCoroutine(ReturnToIdle());
        }

    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1)
        {
            anim.anim.SetTrigger("Idle");
        }
    }
}
