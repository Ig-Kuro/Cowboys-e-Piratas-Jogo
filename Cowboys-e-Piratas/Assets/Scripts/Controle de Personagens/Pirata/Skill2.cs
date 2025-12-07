using System.Collections.Generic;
using UnityEngine;

public class Skill2 : Skill
{
    public Pirata pirata;
    float defaultSpeed;

    private List<GameObject> weapons;

    void Start()
    {
        weapons = pirata.weapons;
        defaultSpeed = pirata.speed;
    }

    public override void Action()
    {
        if (FinishedCooldown())
        {
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
            Invoke(nameof(CmdStartSkill), activationTime);
            //pirata.CmdSetGunState(weapons.IndexOf(pirata.flintKnock.gameObject), true);
            pirata.canAttack = false;
            pirata.canUseSkill2 = false;
            // pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
        }
    }

    public override void CmdEndSkill()
    {
        pirata.speed = defaultSpeed;
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), false);
        pirata.canUlt = true;
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        pirata.canAttack = true;
        pirata.playerUI.Skill2StartCD();
        pirata.state = Pirata.Estado.Normal;
        usando = false;
        currentCooldown = 0;
        ci.inCooldown = true;
        //pirata.CmdSetGunState(weapons.IndexOf(pirata.armaPrincipal.gameObject), true);
    }

    public override void CmdStartSkill()
    {
        pirata.speed /= 2;
        pirata.flintKnock.Action();
        audioStart.Play();
        Invoke(nameof(CmdEndSkill), duration);
        pirata.canUlt = false;
        pirata.canUseSkill1 = false;
        pirata.canAttack = false;
        pirata.state = Pirata.Estado.Atirando;
        usando = true;
    }
}
