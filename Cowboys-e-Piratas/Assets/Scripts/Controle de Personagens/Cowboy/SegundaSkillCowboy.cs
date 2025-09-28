using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    
    private List<GameObject> weapons;

    void Start()
    {
        weapons = cowboy.weapons;
        cowboy.rifle.enabled = false;
    }

    [Command(requiresAuthority = false)]
    public override void Action()
    {
        if (!isLocalPlayer) return;
        if (!FinishedCooldown())
        {
            Debug.Log("Skill ainda em cooldown");
            return;
        }

        // Se já está com rifle, desarma
        if (cowboy.estado == Cowboy.State.Rifle)
        {
            CmdEndSkill();
            return;
        }

        if (ci != null)
        {
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
        }
        
        cowboy.canAttack = false;
        cowboy.canReload = false;
        cowboy.canUlt = false;
        cowboy.canUseSkill1 = false;
        cowboy.primeiraPistola.enabled = false;

        cowboy.StartCoroutine(cowboy.StartRifle());
    }

    [TargetRpc]
    void TargetStartSkill2CD(NetworkConnection target)
    {
        cowboy.playerUI.Skill2StartCD();
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartSkill()
    {
        // Configura rifle após animação
        cowboy.estado = Cowboy.State.Rifle;
        cowboy.armaAtual = cowboy.rifle;

        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
        cowboy.rifle.enabled = true;

        cowboy.canAttack = true;

        usando = true;

        // Agendar retorno automático
        Invoke(nameof(CmdEndSkill), duration);
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndSkill()
    {
        CancelInvoke(nameof(CmdEndSkill));
        cowboy.canAttack = false;
        cowboy.canReload = false;
        cowboy.canUseSkill1 = true;
        cowboy.canUlt = false;
        usando = false;
        ci.inCooldown = true;
        currentCooldown = 0;

        cowboy.StartCoroutine(cowboy.EndRifle());

        TargetStartSkill2CD(connectionToClient);
    }
}
