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
        if (UltReady() && !usando)
        {
            cowboy.anim.anim.SetTrigger("StartUlt");
            Invoke(nameof(CmdStartUltimate), activationTime);
            cowboy.primeiraPistola.gameObject.GetComponent<Gun>().enabled = true;
            cowboy.segundaPistola.gameObject.GetComponent<Gun>().enabled = true;
            cowboy.estado = Cowboy.state.Normal;
            cowboy.armaAtual = cowboy.primeiraPistola;
            cowboy.rifle.gameObject.GetComponent<Gun>().enabled = true;
            cowboy.canUseSkill2 = false;
            cowboy.canUseSkill1 = false;
            cowboy.canReload = false;
        }
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartUltimate()
    {
        cowboy.estado = Cowboy.state.ulting;
        cowboy.primeiraPistola.attackRate = 0.1f;
        cowboy.primeiraPistola.maxAmmo = 9999;
        cowboy.primeiraPistola.recoil = 0;
        cowboy.primeiraPistola.reloadTime = 0;
        cowboy.primeiraPistola.Reload();
        dis.SetActive(true);
        rifle.SetActive(false);
        //audioStart.Play();
        usando = true;
        Invoke(nameof(CmdEndUltimate), duration);
    }



    [Command(requiresAuthority = false)]
    public override void CmdEndUltimate()
    {
        cowboy.anim.anim.SetTrigger("EndUlt");
        Invoke(nameof(ChangeState), 2f);
        cowboy.estado = Cowboy.state.Normal;
        cowboy.segundaPistola.gameObject.GetComponent<Gun>().enabled = false;
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.canAttack = false;
        //audioEnd.Play();
        cowboy.primeiraPistola.attackRate = defaultFireRate;
        cowboy.primeiraPistola.maxAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.currentAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.reloadTime = defaulReloadTime;
        cowboy.primeiraPistola.recoil = defaultRecoil;
        usando = false;
        currentCharge = 0;
    }
    void ChangeState()
    {
        cowboy.canUseSkill2 = true;
        cowboy.canUseSkill1 = true;
        cowboy.canAttack = true;
        cowboy.canReload = true;
        dis.SetActive(false);
        rifle.SetActive(true);
    }

    [Command(requiresAuthority = false)]
    public override void CmdCancelUltimate()
    {
        return;
    }
}
