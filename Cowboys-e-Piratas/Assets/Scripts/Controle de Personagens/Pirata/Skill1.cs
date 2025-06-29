using System.Collections.Generic;
using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill1 : Skill
{
    public Pirata pirata;
    public float activationTime, duration;
    float defaultSpeed;
    public int cura;
    public GameObject curaFX;

    private List<GameObject> weapons;

    void Start()
    {
        weapons = pirata.weapons;
    }

    public override void Action()
    {
        ci.cooldownImage.fillAmount = 0;
        ci.inCooldown = false;
        Invoke(nameof(CmdStartSkill), activationTime);
        defaultSpeed = pirata.speed;
        pirata.canAttack = false;
        pirata.anim.Skill1Pirata();
        pirata.canUseSkill1 = false;
        pirata.canUseSkill2 = false;
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
    }

    public override void CmdEndSkill()
    {
        //audioEnd.Play();
        pirata.speed = defaultSpeed;
        pirata.canAttack = true;
        pirata.state = Pirata.Estado.Normal;
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.jarraDeSuco), false);
        usando = false;
        pirata.canAttack = true;
        pirata.canUseSkill2 = true;
        pirata.canUseSkill1 = true;
        curaFX.SetActive(false);
        ci.inCooldown = true;
        currentCooldown = 0;
    }

    public override void CmdStartSkill()
    {
        Invoke(nameof(CmdEndSkill), duration);
        pirata.speed = 0;
        pirata.canAttack = false;
       // audioStart.Play();
        pirata.state = Pirata.Estado.Curando;
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
       // pirata.CmdSetGunState(weapons.IndexOf(pirata.jarraDeSuco), true);
        usando = true;
        pirata.canUseSkill2 = false;
        pirata.currentHp += cura;
        curaFX.SetActive(true);
        if(pirata.currentHp > pirata.maxHp)
        {
            pirata.currentHp = pirata.maxHp;
        }
        pirata.playerUI.UpdateHP();
    }
}
