using System.Collections.Generic;
using Mirror;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public float duration;
    private List<GameObject> weapons;

    void Start()
    {
        weapons = cowboy.weapons;
        
    }

    [Command(requiresAuthority = false)]
    public override void Action()
    {
        if(FinishedCooldown() && cowboy.estado != Cowboy.State.Rifle)
        {
            // Notifica o dono do personagem para iniciar o cooldown visual
            TargetStartSkill2CD(connectionToClient);
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
            //Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canAttack = false;
            cowboy.canReload = false;
            cowboy.primeiraPistola.gameObject.GetComponent<RangedWeapon>().enabled = false;
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.State.Rifle)
        {
            CmdEndSkill();
        }
        else Debug.Log("Skill n�o carregada");
    }

    [TargetRpc]
    void TargetStartSkill2CD(NetworkConnection target)
    {
        if (cowboy.playerUI != null)
        {
            
        }
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartSkill()
    {
        //valores das variáveis não atualizam para cliente
        cowboy.canAttack = true;
        cowboy.canReload = false;
        usando = true;
        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
        cowboy.rifle.gameObject.GetComponent<RangedWeapon>().enabled = true;
        cowboy.estado = Cowboy.State.Rifle;
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkillAnim), duration);
    }

    void CmdEndSkillAnim()
    {
        cowboy.anim.anim.SetTrigger("EndRifle");
        cowboy.StartCoroutine(cowboy.EndRifle());
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndSkill()
    {
        cowboy.estado = Cowboy.State.Normal;
        cowboy.primeiraPistola.gameObject.GetComponent<RangedWeapon>().enabled = true;
        cowboy.rifle.gameObject.GetComponent<RangedWeapon>().enabled = false;
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.playerUI.Skill2StartCD();
        cowboy.canUseSkill1 = true;
        usando = false;
        ci.inCooldown = true;
        cowboy.canReload = true;
        currentCooldown = 0;
    }
}
