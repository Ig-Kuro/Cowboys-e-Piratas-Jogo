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
            //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
            pirata.polvoSummon.SetActive(true);
            summonPolvo.areaVizualizer = Instantiate(summonPolvo.areaVizualizerPrefab, new Vector3(-100, -100, 100), Quaternion.identity);
            pirata.canAttack = false;
            pirata.canUseSkill1 = false;
            pirata.canUseSkill2 = false;
            usando = true;
            pirata.anim.anim.SetTrigger("Ult");
            pirata.anim.anim.SetBool("Ultando", true);
            pirata.state = Pirata.Estado.Ultando;
        }
    }

    public override void CmdCancelUltimate()
    {
        pirata.polvoSummon.SetActive(false);

        if (summonPolvo.areaVizualizer != null)
            NetworkServer.Destroy(summonPolvo.areaVizualizer);

        pirata.state = Pirata.Estado.Normal;
        Destroy(summonPolvo.areaVizualizer);
        pirata.anim.anim.SetBool("Ultando", false);
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        usando = false;
    }

    public override void CmdEndUltimate()
    {
        if (polvoSpawnado != null)
            NetworkServer.Destroy(polvoSpawnado);
        usando = false;
        currentCharge = 0;
        pirata.anim.anim.SetBool("Ultando", false);
    }

    public override void CmdStartUltimate()
    {
        Destroy(summonPolvo.areaVizualizer);
        GameObject polvoObj = Instantiate(polvo, summonPolvo.areaVizualizer.transform.position, Quaternion.identity);
        NetworkServer.Spawn(polvoObj);
        polvoSpawnado = polvoObj;
        Invoke(nameof(CmdEndUltimate), duration);
        pirata.polvoSummon.SetActive(false);
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        pirata.state = Pirata.Estado.Normal;
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        pirata.anim.anim.SetBool("Ultando", false);
    }
}
