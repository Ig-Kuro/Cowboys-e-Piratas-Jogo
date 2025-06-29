using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class UltimatePirata : Ultimate
{
    public Pirata pirata;
    public SummonPolvo summonPolvo;
    public float activationTime;
    public GameObject polvo, polvoSpawnado;

    private List<GameObject> weapons;

    private Vector3 spawnPosition;

    private bool ultConfirmed = false;

    void Start()
    {
        weapons = pirata.weapons;
    }

    public override void Action()
    {
        if (UltReady() && !usando)
        {
            pirata.skill1.CmdEndSkill();
            pirata.skill2.CmdEndSkill();

            pirata.canAttack = false;
            pirata.canUseSkill1 = false;
            pirata.canUseSkill2 = false;

            usando = true;
            pirata.anim.anim.SetTrigger("Ult");
            pirata.state = Pirata.Estado.Ultando;

            pirata.polvoSummon.SetActive(true);
            summonPolvo.areaVizualizer = Instantiate(summonPolvo.areaVizualizerPrefab, new Vector3(-100, -100, 100), Quaternion.identity);
        }
    }

    [Command(requiresAuthority = false)]
    public override void CmdCancelUltimate()
    {
        pirata.polvoSummon.SetActive(false);

        if (summonPolvo.areaVizualizer != null)
            Destroy(summonPolvo.areaVizualizer);

        pirata.state = Pirata.Estado.Normal;
        pirata.anim.anim.SetTrigger("EndUlt");
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        usando = false;
    }

    [Command(requiresAuthority = false)]
    public override void CmdEndUltimate()
    {
        if (polvoSpawnado != null)
            NetworkServer.Destroy(polvoSpawnado);

        ultConfirmed = false;
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartUltimate()
    {
        Debug.Log("Servidor executou CmdStartUltimate");
        Debug.Log("spawnPosition: " + summonPolvo.visualizerPosition);
        if (ultConfirmed || summonPolvo.areaVizualizer == null)
            return;

        spawnPosition = summonPolvo.visualizerPosition;

        pirata.state = Pirata.Estado.Normal;

        GameObject polvoObj = Instantiate(polvo, spawnPosition, Quaternion.identity);
        polvoSpawnado = polvoObj;
        Debug.Log(spawnPosition);
        polvoSpawnado.GetComponent<PolvoAtaque>().SetPosition(spawnPosition);
        NetworkServer.Spawn(polvoObj);

        Invoke(nameof(CmdEndUltimate), duration);

        
        DisableClientStuff();
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);

        ultConfirmed = true;
        
    }

    [TargetRpc]
    void DisableClientStuff()
    {
        usando = false;
        currentCharge = 0;
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        pirata.polvoSummon.SetActive(false);
        
        pirata.anim.anim.SetTrigger("EndUlt");

        if (summonPolvo.areaVizualizer != null)
            Destroy(summonPolvo.areaVizualizer);
    }
}
