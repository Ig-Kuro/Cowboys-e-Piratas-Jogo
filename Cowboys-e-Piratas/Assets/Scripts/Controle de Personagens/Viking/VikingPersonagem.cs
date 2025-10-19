using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class VikingPersonagem : Personagem
{
    public enum Estado { Normal, Gritando, Ultando, Porradando }
    public Estado state;
    public MeleeWeapon axe;
    public MeleeWeapon crystalWave;
    public GameObject crystalWaveFX;
    public int damgeMultiplier = 1;
    public int drainHp;
    public GameObject cam1, cam2;

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
        canAttack = false;

        yield return new WaitUntil(() => anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.8f &&
        anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Ataque"));

        axe.CmdPerformAttack(transform.position, 1 * damgeMultiplier, transform.forward);
        StopCoroutine(ReturnToIdle());
        if(axe.killedEnemy && state == Estado.Ultando)
        {
            DrainHp();
        }
        
        canAttack = true;
    }


    IEnumerator GritoAnimation()
    {
        StartCoroutine(ReturnToIdle());

        yield return new WaitUntil(() => anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f &&
        anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Stun"));

        skill1.CmdStartSkill();
        skill1.CmdEndSkill();
        StopCoroutine(ReturnToIdle());
    }

    IEnumerator PorradaoAnimation()
    {
        yield return new WaitUntil(()=> anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.8f &&
        anim.anim.GetCurrentAnimatorStateInfo(1).IsName("Porradao"));

        axe.CmdPerformAttack(transform.position, 1 * damgeMultiplier, transform.forward);
        crystalWave.CmdPerformAttack(crystalWave.transform.position, 1 * damgeMultiplier, transform.forward);
        Instantiate(crystalWaveFX, crystalWave.transform.position, Quaternion.identity);
        skill2.CmdEndSkill();
        StopCoroutine(ReturnToIdle());
       
        
    }

    IEnumerator UltimateStart()
    {
        yield return new WaitUntil(() => anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f &&
        anim.anim.GetCurrentAnimatorStateInfo(1).IsName("StartUlt"));

        ult.CmdStartUltimate();
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


    public void DrainHp()
    {
        for (int i = 0; i < axe.enemiesKilled; i++)
        {
            currentHp += drainHp;
            if(currentHp > maxHp)
            {
                currentHp = maxHp;
            }
        }
    }

}
