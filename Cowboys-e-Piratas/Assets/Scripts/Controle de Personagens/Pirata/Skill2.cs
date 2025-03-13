using UnityEngine;

public class Skill2 : Skill
{
    public override void Action()
    {
        Debug.Log("Skill 2 º-º");
    }

    public override void EndSkill()
    {
        throw new System.NotImplementedException();
    }

    public override void StartSkill()
    {
        throw new System.NotImplementedException();
    }
}
