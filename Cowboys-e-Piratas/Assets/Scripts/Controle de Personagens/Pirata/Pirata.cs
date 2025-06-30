using UnityEngine;

public class Pirata : Personagem
{
    public enum Estado { Normal, Curando, Ultando, Atirando }
    public Estado state;

    public GameObject jarraDeSuco, polvoSummon;
    public Arma flintKnock;
    public MeleeWeapon weapon;
    public float buffer;
    private float timer;
    private bool attacBuffer, skill1Buffer, skill2Buffer, ultBuffer;

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
            skill2.Action();
            //UIManagerPirata.instance.Skill2StartCD();
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
}
