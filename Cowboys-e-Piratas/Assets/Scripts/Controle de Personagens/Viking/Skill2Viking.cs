using UnityEngine;

public class Skill2Viking : Skill
{
    public VikingPersonagem viking;
    public override void Action()
    {
        viking.canAttack = false;
        viking.canUseSkill1 = false;
        viking.state = VikingPersonagem.Estado.Porradando;
        usando = true;
    }

    public override void CmdEndSkill()
    {
        viking.canAttack = true;
        viking.canUseSkill1 = true;
        viking.state = VikingPersonagem.Estado.Normal;
        usando = false;
        currentCooldown = 0;
    }   
}
