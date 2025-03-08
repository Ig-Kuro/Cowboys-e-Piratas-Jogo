using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public float duration;
    public override void Action()
    {
        if(FinishedCooldown() && cowboy.estado != Cowboy.state.skill2)
        {
            Invoke("StartSkill", activationTime);
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.state.skill2)
        {
            EndSkill();
        }
        else Debug.Log("Skill não carregada");
    }

    public override void StartSkill()
    {
        cowboy.estado = Cowboy.state.skill2;
        cowboy.primeiraPistola.gameObject.SetActive(false);
        cowboy.rifle.gameObject.SetActive(true);
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;
        Invoke("EndSkill", duration);
    }

    public override void EndSkill()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.primeiraPistola.gameObject.SetActive(true);
        cowboy.rifle.gameObject.SetActive(false);
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.canUseSkill1 = true;
        currentCooldown = 0;
    }
}
