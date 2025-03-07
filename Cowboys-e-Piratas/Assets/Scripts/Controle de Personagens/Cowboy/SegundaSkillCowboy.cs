using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public override void Action()
    {
        Invoke("StartSkill", activationTime);
    }

    public void StartSkill()
    {
        cowboy.estado = Cowboy.state.skill2;
        cowboy.primeiraPistola.gameObject.SetActive(false);
        cowboy.rifle.gameObject.SetActive(true);
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;

    }
}
