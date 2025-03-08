using JetBrains.Annotations;
using UnityEngine;

public class PrimeiraSkillCowboy : Skill
{
    public float activationTime;
    public override void Action()
    {

        if (FinishedCooldown())
        {
            Invoke("StartSkill", activationTime);
        }
        else Debug.Log("Skill não carregada");
    }
    public override void StartSkill()
    {
    }

    public override void EndSkill()
    {
    }

}
