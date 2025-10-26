using UnityEngine;

public class Skill2Viking : Skill
{
    public VikingPersonagem viking;
    
    public override void Action()
    {
        viking.canTakeDamage = false;
        viking.canAttack = false;
        viking.canUseSkill1 = false;
        viking.canUseSkill2 = false;
        viking.movement.canMove = false;
        viking.state = VikingPersonagem.Estado.Porradando;
        usando = true;
        viking.clippingMesh.SetActive(true);
        viking.cam1.SetActive(false);
        viking.cam2.SetActive(true);
    }

    public override void CmdEndSkill()
    {
        viking.cam2.SetActive(false);
        viking.cam1.SetActive(true);
        if (isLocalPlayer)
            viking.clippingMesh.SetActive(false);
        Debug.Log("fui");
        viking.canAttack = true;
        viking.canUseSkill1 = true;
        viking.canUseSkill2 = true;
        viking.movement.canMove = true;
        viking.state = VikingPersonagem.Estado.Normal;
        usando = false;
        currentCooldown = 0;
        viking.canTakeDamage = true;
            
    }   
}
