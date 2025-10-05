using UnityEngine;

public class Skill2Viking : Skill
{
    public VikingPersonagem viking;
    public GameObject cam1, cam2;
    public override void Action()
    {
        viking.canAttack = false;
        viking.canUseSkill1 = false;
        viking.canUseSkill2 = false;
        viking.state = VikingPersonagem.Estado.Porradando;
        usando = true;
        viking.clippingMesh.SetActive(true);
        cam1.GetComponent<Camera>().enabled = false;
        cam2.SetActive(true);
    }

    public override void CmdEndSkill()
    {
        cam2.SetActive(false);
        cam1.GetComponent<Camera>().enabled = true;
        Debug.Log("fui");
        viking.canAttack = true;
        viking.canUseSkill1 = true;
        viking.canUseSkill2 = true;
        viking.state = VikingPersonagem.Estado.Normal;
        usando = false;
        currentCooldown = 0;
            if(isLocalPlayer)
            viking.clippingMesh.SetActive(false);
    }   
}
