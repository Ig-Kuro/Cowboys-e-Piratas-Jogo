using Mirror;
using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public float duration;
    public override void Action()
    {
        if(FinishedCooldown() && cowboy.estado != Cowboy.state.rifle)
        {
            Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canAttack = false;
            cowboy.canReload = false;
            cowboy.RpcSetGunState(cowboy.primeiraPistola.gameObject, false);
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.state.rifle)
        {
            CmdEndSkill();
        }
        else Debug.Log("Skill n�o carregada");
    }

    public override void CmdStartSkill()
    {
        cowboy.canAttack = true;
        cowboy.canReload = true;
        usando = true;
        cowboy.estado = Cowboy.state.rifle;
        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
        cowboy.RpcSetGunState(cowboy.rifle.gameObject, true);
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkill), duration);
    }

    public override void CmdEndSkill()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.RpcSetGunState(cowboy.primeiraPistola.gameObject, true);
        cowboy.RpcSetGunState(cowboy.rifle.gameObject, false);
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.canUseSkill1 = true;
        usando = false;
        currentCooldown = 0;
    }
}
