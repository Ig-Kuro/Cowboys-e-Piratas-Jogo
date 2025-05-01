using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill1 : Skill
{
    public Pirata pirata;
    public float activationTime, duration;
    float defaultSpeed;
    public int cura;
    public override void Action()
    {
        Invoke(nameof(CmdStartSkill), activationTime);
        defaultSpeed = pirata.speed;
        pirata.canAttack = false;
        pirata.armaPrincipal.gameObject.SetActive(false);
        pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.SetActive(false);
    }

    public override void CmdEndSkill()
    {
        pirata.speed = defaultSpeed;
        pirata.canAttack = true;
        pirata.state = Pirata.Estado.Normal;
        pirata.armaPrincipal.gameObject.SetActive(true);
        pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.SetActive(true);
        pirata.jarraDeSuco.SetActive(false);
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
        pirata.state = Pirata.Estado.Curando;
        pirata.armaPrincipal.gameObject.SetActive(false);
        pirata.jarraDeSuco.SetActive(true);
        usando = true;
        pirata.canUseSkill2 = false;
        pirata.currentHp += cura;
        if(pirata.currentHp > pirata.maxHp)
        {
            pirata.currentHp = pirata.maxHp;
        }
    }
}
