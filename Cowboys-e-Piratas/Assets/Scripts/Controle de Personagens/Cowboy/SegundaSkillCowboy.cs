using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public float duration;
    private List<Arma> weapons;

    void Start()
    {
        weapons = cowboy.weapons;
    }

    [Command(requiresAuthority = false)]
    public override void Action()
    {
        if(FinishedCooldown() && cowboy.estado != Cowboy.state.rifle)
        {
            Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canAttack = false;
            cowboy.canReload = false;
            cowboy.CmdSetGunState(weapons.IndexOf(cowboy.primeiraPistola), false);
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.state.rifle)
        {
            CmdEndSkill();
        }
        else Debug.Log("Skill n�o carregada");
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartSkill()
    {
        //valores das variáveis não atualizam para cliente
        cowboy.canAttack = true;
        cowboy.canReload = true;
        usando = true;
        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
        cowboy.CmdSetGunState(weapons.IndexOf(cowboy.rifle), true);
        cowboy.estado = Cowboy.state.rifle;
        cowboy.rifleCostas.SetActive(false);
        //audioStart.Play();
        cowboy.rifleMao.SetActive(true);
        cowboy.armaAtual = cowboy.rifle;
        //UIManagerCowboy.instance.AttAmmo(cowboy.rifle);
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkill), duration);
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndSkill()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.CmdSetGunState(weapons.IndexOf(cowboy.primeiraPistola), true);
        cowboy.CmdSetGunState(weapons.IndexOf(cowboy.rifle), false);
        cowboy.rifleCostas.SetActive(true);
        cowboy.rifleMao.SetActive(false);
        //audioEnd.Play();
        cowboy.armaAtual = cowboy.primeiraPistola;
        //UIManagerCowboy.instance.AttAmmo(cowboy.primeiraPistola);
        cowboy.canUseSkill1 = true;
        usando = false;
        currentCooldown = 0;
    }
}
