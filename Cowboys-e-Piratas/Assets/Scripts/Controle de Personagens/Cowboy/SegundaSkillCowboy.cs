using System.Collections.Generic;
using Mirror;
using UnityEngine;

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
            
            Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canAttack = false;
            cowboy.canReload = false;
            cowboy.primeiraPistola.gameObject.GetComponent<Gun>().enabled = false;
            cowboy.anim.anim.SetTrigger("StartRifle");
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
            cowboy.playerUI.Skill2StartCD();
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
        cowboy.rifle.gameObject.GetComponent<Gun>().enabled = true;
        cowboy.estado = Cowboy.State.Rifle;
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkill), duration);
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndSkill()
    {
        cowboy.estado = Cowboy.State.Normal;
        cowboy.primeiraPistola.gameObject.GetComponent<Gun>().enabled = true;
        cowboy.rifle.gameObject.GetComponent<Gun>().enabled = false;
        cowboy.anim.anim.SetTrigger("EndRifle");
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.canUseSkill1 = true;
        usando = false;
        cowboy.canReload = true;
        currentCooldown = 0;
    }
}
