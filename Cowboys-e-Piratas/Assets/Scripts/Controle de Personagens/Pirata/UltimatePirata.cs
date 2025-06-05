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

    void Start()
    {
        weapons = pirata.weapons;
    }

    public override void Action()
    {
        if(UltReady() && !usando)
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
        Destroy(summonPolvo.areaVizualizer);
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
        usando = false;
        currentCharge = 0;
        pirata.anim.anim.SetTrigger("EndUlt");
    }

    [Command(requiresAuthority = false)]
    public override void CmdStartUltimate()
    {
        if (summonPolvo.areaVizualizer == null) return;

        Vector3 spawnPosition = summonPolvo.areaVizualizer.transform.position;
        GameObject polvoObj = Instantiate(polvo, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(polvoObj);
        polvoSpawnado = polvoObj;

        Destroy(summonPolvo.areaVizualizer); // Remove apenas do cliente local
        pirata.polvoSummon.SetActive(false);

        Invoke(nameof(CmdEndUltimate), duration);

        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        pirata.state = Pirata.Estado.Normal;
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        pirata.anim.anim.SetTrigger("EndUlt");
    }
}
