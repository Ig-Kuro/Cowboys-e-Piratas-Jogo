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

    private List<GameObject> weapons;

    void Start()
    {
        weapons = pirata.weapons;
    }

    public override void Action()
    {
        Invoke(nameof(CmdStartSkill), activationTime);
        defaultSpeed = pirata.speed;
        pirata.canAttack = false;

        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
    }

    public override void CmdEndSkill()
    {
        //audioEnd.Play();
        pirata.speed = defaultSpeed;
        pirata.canAttack = true;
        pirata.state = Pirata.Estado.Normal;
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
        pirata.CmdSetGunState(weapons.IndexOf(pirata.jarraDeSuco), false);
        usando = false;
        pirata.canAttack = true;
        pirata.canUseSkill2 = true;
        currentCooldown = 0;
    }

    public override void CmdStartSkill()
    {
        Invoke(nameof(CmdEndSkill), duration);
        pirata.speed = 0;
        pirata.canAttack = false;
       // audioStart.Play();
        pirata.state = Pirata.Estado.Curando;
        pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
        pirata.CmdSetGunState(weapons.IndexOf(pirata.jarraDeSuco), true);
        usando = true;
        pirata.canUseSkill2 = false;
        pirata.currentHp += cura;
        if(pirata.currentHp > pirata.maxHp)
        {
            pirata.currentHp = pirata.maxHp;
        }
    }
}
