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
            cowboy.primeiraPistola.gameObject.SetActive(false);
            //cowboy.RpcSetGunState(cowboy.primeiraPistola.gameObject, false);
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
        //cowboy.RpcSetGunState(cowboy.rifle.gameObject, true);
        cowboy.rifle.gameObject.SetActive(true);
        cowboy.rifleCostas.SetActive(false);
        cowboy.rifleMao.SetActive(true);
        cowboy.armaAtual = cowboy.rifle;
        UIManagerCowboy.instance.AttAmmo(cowboy.rifle);
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkill), duration);
    }

    public override void CmdEndSkill()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.rifle.gameObject.SetActive(false);
        cowboy.rifleCostas.SetActive(true);
        cowboy.rifleMao.SetActive(false);
        cowboy.primeiraPistola.gameObject.SetActive(true);
        // cowboy.RpcSetGunState(cowboy.primeiraPistola.gameObject, true);
        //cowboy.RpcSetGunState(cowboy.rifle.gameObject, false);
        cowboy.armaAtual = cowboy.primeiraPistola;
        UIManagerCowboy.instance.AttAmmo(cowboy.primeiraPistola);
        cowboy.canUseSkill1 = true;
        usando = false;
        currentCooldown = 0;
    }
}
