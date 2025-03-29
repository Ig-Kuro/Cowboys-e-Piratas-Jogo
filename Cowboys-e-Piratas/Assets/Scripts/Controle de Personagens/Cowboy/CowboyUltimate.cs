using UnityEngine;
using System.Collections.Generic;

public class CowboyUltimate : Ultimate
{
    public Cowboy cowboy;
    public float activationTime;
    float defaultFireRate;
    int defaultMaxAmmo;
    float defaulReloadTime;
    float defaultRecoil;
    private List<Arma> weapons;

    private void Start()
    {
        weapons = cowboy.weapons;
        defaulReloadTime = cowboy.primeiraPistola.reloadTime;
        defaultFireRate = cowboy.primeiraPistola.attackRate;
        defaultMaxAmmo = cowboy.primeiraPistola.maxAmmo;
        defaultRecoil = cowboy.primeiraPistola.recoil; 
    }
    public override void Action()
    {
        if (Carregado() && !usando)
        {
            Invoke(nameof(CmdStartUltimate), activationTime);
            cowboy.RpcSetGunState(weapons.IndexOf(cowboy.primeiraPistola), true);
            cowboy.RpcSetGunState(weapons.IndexOf(cowboy.segundaPistola), true);
            cowboy.estado = Cowboy.state.Normal;
            cowboy.armaAtual = cowboy.primeiraPistola;
            cowboy.rifle.gameObject.SetActive(false);
            cowboy.canUseSkill2 = false;
            cowboy.canUseSkill1 = false;
        }
        else Debug.Log("Ult n�o carregada");
    }

    public override void CmdStartUltimate()
    {
        cowboy.estado = Cowboy.state.ulting;
        cowboy.primeiraPistola.attackRate = 0.1f;
        cowboy.primeiraPistola.maxAmmo = 9999;
        cowboy.primeiraPistola.recoil = 0;
        cowboy.primeiraPistola.reloadTime = 0;
        cowboy.primeiraPistola.Reload();
        usando = true;
        Invoke(nameof(CmdEndUltimate), duration);
    }

    public override void CmdEndUltimate()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.RpcSetGunState(weapons.IndexOf(cowboy.segundaPistola), false);
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.primeiraPistola.attackRate = defaultFireRate;
        cowboy.primeiraPistola.maxAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.currentAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.reloadTime = defaulReloadTime;
        cowboy.primeiraPistola.recoil = defaultRecoil;
        cowboy.canUseSkill2 = true;
        cowboy.canUseSkill1 = true;
        usando = false;
        currentCharge = 0;
    }

    public override void CmdCancelUltimate()
    {
        throw new System.NotImplementedException();
    }
}
