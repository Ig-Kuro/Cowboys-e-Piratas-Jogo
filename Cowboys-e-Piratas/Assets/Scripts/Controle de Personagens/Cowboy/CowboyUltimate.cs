using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class CowboyUltimate : Ultimate
{
    public Cowboy cowboy;
    public float activationTime;
    float defaultFireRate;
    int defaultMaxAmmo;
    float defaulReloadTime;
    float defaultRecoil;
    private List<GameObject> weapons;

    public GameObject dis, rifle;

    RangedWeapon mainPistol;
    private void Start()
    {
        weapons = cowboy.weapons;
        mainPistol = cowboy.primeiraPistola;
        defaulReloadTime = mainPistol.reloadTime;
        defaultFireRate = mainPistol.attackRate;
        defaultMaxAmmo = mainPistol.maxAmmo;
        defaultRecoil = mainPistol.recoil;
    }
    public override void Action()
    {
        if (UltReady() && !usando)
        {
            cowboy.anim.anim.SetTrigger("StartUlt");
            cowboy.canAttack = false;
            Invoke(nameof(CmdStartUltimate), activationTime);
            mainPistol.enabled = true;
            cowboy.segundaPistola.enabled = true;
            cowboy.armaAtual = mainPistol;
            cowboy.rifle.enabled = true;
            cowboy.canUseSkill2 = false;
            cowboy.canUseSkill1 = false;
            cowboy.canReload = false;
        }
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartUltimate()
    {
        cowboy.estado = Cowboy.State.Ulting;
        cowboy.canAttack = true;
        mainPistol.attackRate = 0.1f;
        mainPistol.maxAmmo = 9999;
        mainPistol.recoil = 0;
        mainPistol.reloadTime = 0;
        mainPistol.Reload();
        cowboy.CmdSetGunState(weapons.IndexOf(dis), true);
        cowboy.CmdSetGunState(weapons.IndexOf(rifle), false);
        //audioStart.Play();
        usando = true;
        Invoke(nameof(CmdEndUltimate), duration);
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndUltimate()
    {
        cowboy.anim.anim.SetTrigger("EndUlt");
        Invoke(nameof(ChangeState), 1f);
        cowboy.segundaPistola.enabled = false;
        cowboy.armaAtual = mainPistol;
        cowboy.canAttack = false;
        //audioEnd.Play();
        mainPistol.attackRate = defaultFireRate;
        mainPistol.maxAmmo = defaultMaxAmmo;
        mainPistol.currentAmmo = defaultMaxAmmo;
        mainPistol.reloadTime = defaulReloadTime;
        mainPistol.recoil = defaultRecoil;
        usando = false;
        currentCharge = 0;
    }
    void ChangeState()
    {
        cowboy.canUseSkill2 = true;
        cowboy.canUseSkill1 = true;
        cowboy.canAttack = true;
        cowboy.canReload = true;
        cowboy.CmdSetGunState(weapons.IndexOf(dis), false);
        cowboy.CmdSetGunState(weapons.IndexOf(rifle), true);
        cowboy.estado = Cowboy.State.Normal;
        cowboy.RestartReturnToIdle();
    }
}
