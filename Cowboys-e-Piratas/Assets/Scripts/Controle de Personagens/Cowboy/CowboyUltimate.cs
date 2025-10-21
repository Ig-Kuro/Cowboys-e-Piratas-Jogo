using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;

public class CowboyUltimate : Ultimate
{
    public Cowboy cowboy;
    public float activationTime;
    public GameObject dis, rifle;

    private RangedWeapon mainPistol;
    private List<GameObject> weapons;

    float defaultFireRate;
    int defaultMaxAmmo;
    float defaultReloadTime;
    float defaultRecoil;

    private void Start()
    {
        weapons = cowboy.weapons;
        mainPistol = cowboy.primeiraPistola;
        defaultReloadTime = mainPistol.reloadTime;
        defaultFireRate = mainPistol.attackRate;
        defaultMaxAmmo = mainPistol.maxAmmo;
        defaultRecoil = mainPistol.recoil;
    }

    public override void Action()
    {
        if (!UltReady() || usando) return;

        cowboy.anim.anim.SetTrigger("StartUlt");
        cowboy.canAttack = false;
        cowboy.canUseSkill1 = false;
        cowboy.canUseSkill2 = false;
        cowboy.canReload = false;

        //Em vez de Invoke local, manda o comando pro servidor
        CmdRequestStartUltimate();
    }

    [Command]
    private void CmdRequestStartUltimate(NetworkConnectionToClient sender = null)
    {
        //Servidor agenda o início real da ultimate
        StartCoroutine(StartUltimateAfterDelay());
    }

    private IEnumerator StartUltimateAfterDelay()
    {
        yield return new WaitForSeconds(activationTime);
        CmdStartUltimate();
    }

    [Server]
    public override void CmdStartUltimate()
    {
        cowboy.estado = Cowboy.State.Ulting;
        cowboy.canAttack = true;

        mainPistol.enabled = true;
        cowboy.segundaPistola.enabled = true;
        mainPistol.attackRate = 0.1f;
        mainPistol.maxAmmo = 9999;
        mainPistol.recoil = 0;
        mainPistol.reloadTime = 0;
        mainPistol.Reload();

        cowboy.CmdSetGunState(weapons.IndexOf(dis), true);
        cowboy.CmdSetGunState(weapons.IndexOf(rifle), false);

        usando = true;

        //Agendar término no servidor
        Invoke(nameof(CmdEndUltimate), duration);

        //Notifica os clientes para efeitos visuais
        RpcOnStartUltimate();
    }

    [ClientRpc]
    private void RpcOnStartUltimate()
    {
        // Ativa animações ou efeitos visuais para todos os clientes
        cowboy.anim.anim.SetTrigger("StartUlt");
    }

    [Server]
    public override void CmdEndUltimate()
    {
        cowboy.anim.anim.SetTrigger("EndUlt");
        Invoke(nameof(ChangeState), 1f);

        cowboy.segundaPistola.enabled = false;
        cowboy.armaAtual = mainPistol;
        cowboy.canAttack = false;

        mainPistol.attackRate = defaultFireRate;
        mainPistol.maxAmmo = defaultMaxAmmo;
        mainPistol.currentAmmo = defaultMaxAmmo;
        mainPistol.reloadTime = defaultReloadTime;
        mainPistol.recoil = defaultRecoil;

        usando = false;
        currentCharge = 0;

        RpcOnEndUltimate();
    }

    [ClientRpc]
    private void RpcOnEndUltimate()
    {
        // Sincroniza fim da ult nos clientes
        cowboy.anim.anim.SetTrigger("EndUlt");
    }

    [Server]
    private void ChangeState()
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
