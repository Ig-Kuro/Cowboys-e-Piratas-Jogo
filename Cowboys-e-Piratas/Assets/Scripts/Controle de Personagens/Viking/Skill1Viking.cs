using System.Collections.Generic;
using UnityEngine;

public class Skill1Viking : Skill
{
    public float checkRadius;
    public static List<Inimigo> inims = new List<Inimigo>();
    public VikingPersonagem viking;
    public override void Action()
    {
        viking.canAttack = false;
        viking.canUseSkill1 = false;
        viking.state = VikingPersonagem.Estado.Gritando;
        usando = true;
    }

    public override void CmdStartSkill()
    {
        GetEnemies();
    }
    private void GetEnemies()
    {
        Collider[] colider = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider col in colider)
        {
            if (col.gameObject.GetComponent<Inimigo>() != null)
            {
                inims.Add(col.gameObject.GetComponent<Inimigo>());
            }
        }
        //audioEnd.Play();
        StunEnemies();
    }

    void StunEnemies()
    {
        if (inims.Count > 0)
        {
            foreach (Inimigo it in inims)
            {
                it.Stun();
            }
            inims.RemoveAll(item => item == null);
        }

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
