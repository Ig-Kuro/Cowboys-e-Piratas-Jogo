using Mirror;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class NinjaSecondSkill : Skill
{
    public NinjaPersonagem ninja;
    public override void Action()
    {
        if (FinishedCooldown())
        {
            TargetStartSkill2CD(connectionToClient);
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
            ninja.estado = NinjaPersonagem.State.Melee;
            Debug.Log("Skill2");


            ninja.canAttack = false;
            ninja.canUseSkill1 = false;
        }
    }

    public override void CmdEndSkill()
    {
        ninja.canAttack =  true;
        ninja.canUseSkill1 = true;
        currentCooldown = 0;
        ninja.estado = NinjaPersonagem.State.Normal;
    }







    void TargetStartSkill2CD(NetworkConnection target)
    {
        if (ninja.playerUI != null)
        {

        }
    }
}
