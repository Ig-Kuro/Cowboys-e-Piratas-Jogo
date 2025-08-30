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
            skill1.Action();
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
        while (anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1 && !anim.anim.GetCurrentAnimatorStateInfo(1).IsName("ossos pirata|AtirandoPistola/Braços"))
        {
            yield return new WaitForEndOfFrame();
        }

        skill2.Action();
    }
}
