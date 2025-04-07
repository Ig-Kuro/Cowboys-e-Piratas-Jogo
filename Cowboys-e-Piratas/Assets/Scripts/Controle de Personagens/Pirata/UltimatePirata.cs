using UnityEngine;

public class UltimatePirata : Ultimate
{
    public Pirata pirata;
    public SummonPolvo summonPolvo;
    public float activationTime;
    public GameObject polvo, polvoSpawnado;
    public override void Action()
    {
        if(Carregado() && !usando)
        {
            pirata.skill1.CmdEndSkill();
            pirata.jarraDeSuco.SetActive(false);
            pirata.skill2.CmdEndSkill();
            pirata.polvoSummon.SetActive(true);
            pirata.flintKnock.gameObject.SetActive(false);
            summonPolvo.areaVizualizer = Instantiate(summonPolvo.areaVizualizerPrefab, new Vector3(-100, -100, 100), Quaternion.identity);
            pirata.armaPrincipal.gameObject.SetActive(false);
            pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.gameObject.SetActive(false);
            pirata.canAttack = false;
            pirata.canUseSkill1 = false;
            pirata.canUseSkill2 = false;
            usando = true;
            pirata.state = Pirata.Estado.Ultando;
        }
    }


    public override void CmdCancelUltimate()
    {
        pirata.polvoSummon.SetActive(false);
        Destroy(summonPolvo.areaVizualizer);
        pirata.state = Pirata.Estado.Normal;
        pirata.armaPrincipal.gameObject.SetActive(true);
        pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.gameObject.SetActive(true);
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        usando = false;
    }
    public override void CmdEndUltimate()
    {
        Destroy(polvoSpawnado);
        usando = false;
        currentCharge = 0;
    }

    public override void CmdStartUltimate()
    {
        polvoSpawnado = Instantiate(polvo, summonPolvo.areaVizualizer.transform.position, Quaternion.identity);
        Invoke("EndUltimate", duration);
        pirata.polvoSummon.SetActive(false);
        pirata.armaPrincipal.gameObject.SetActive(true);
        pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.gameObject.SetActive(true);
        Destroy(summonPolvo.areaVizualizer);
        pirata.state = Pirata.Estado.Normal;
        pirata.canAttack = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
    }
}
