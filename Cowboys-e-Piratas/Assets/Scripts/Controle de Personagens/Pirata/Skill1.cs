using Mirror.Examples.Basic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill1 : Skill
{
    public GameObject player;
    public override void Action()
    {
        player.GetComponent<Pirata>().speed=0;
        player.GetComponent<Pirata>().canAttack=false;
        
        player.GetComponent<Pirata>().hp+=5;
        if(player.GetComponent<Pirata>().hp<10f)
        {
            player.GetComponent<Pirata>().hp -= (player.GetComponent<Pirata>().hp-10);
        }
        Invoke(nameof(Healed),0.5f);
    }

    public override void EndSkill()
    {
        throw new System.NotImplementedException();
    }

    public override void StartSkill()
    {
        throw new System.NotImplementedException();
    }

    private void Healed()
    {
        player.GetComponent<Pirata>().speed=4;
        player.GetComponent<Pirata>().canAttack=true;
    }
}
