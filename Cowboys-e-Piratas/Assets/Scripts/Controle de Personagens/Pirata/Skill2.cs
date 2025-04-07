using UnityEngine;

public class Skill2 : Skill
{
    public Pirata pirata;
    public float activationTime, duration;
    float defaultSpeed;
    public override void Action()
    {
        if (FinishedCooldown())
        {
            Invoke(nameof(CmdStartSkill), activationTime);
            defaultSpeed = pirata.speed;
            pirata.flintKnock.gameObject.SetActive(true);
            pirata.canAttack = false;
            pirata.armaPrincipal.gameObject.SetActive(false);
            pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.gameObject.SetActive(false);
        }
    }

    public override void CmdEndSkill()
    {
        pirata.speed = defaultSpeed;
        pirata.flintKnock.gameObject.SetActive(false);
        pirata.armaPrincipal.GetComponent<MeleeWeapon>().espada.gameObject.SetActive(true);
        pirata.canUlt = true;
        pirata.canUseSkill1 = true;
        pirata.canAttack = true;
        pirata.state = Pirata.Estado.Normal;
        usando = false;
        currentCooldown = 0;
        pirata.armaPrincipal.gameObject.SetActive(true);
    }

    public override void CmdStartSkill()
    {
        pirata.speed /= 2;
        pirata.flintKnock.Action();
        Invoke(nameof(CmdEndSkill), duration);
        pirata.canUlt = false;
        pirata.canUseSkill1 = false;
        pirata.canAttack = false;
        pirata.state = Pirata.Estado.Atirando;
        usando = true;
    }
}
