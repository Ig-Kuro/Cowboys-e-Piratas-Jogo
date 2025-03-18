using UnityEngine;

public class SegundaSkillCowboy : Skill
{
    public Cowboy cowboy;
    public float activationTime;
    public float duration;
    public override void Action()
    {
        if(FinishedCooldown() && cowboy.estado != Cowboy.state.rifle)
        {
            Invoke("StartSkill", activationTime);
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.state.rifle)
        {
            EndSkill();
        }
        else Debug.Log("Skill não carregada");
    }

    public override void StartSkill()
    {
        usando = true;
        cowboy.estado = Cowboy.state.rifle;
        cowboy.primeiraPistola.gameObject.SetActive(false);
        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
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
        usando = false;
        currentCooldown = 0;
    }
}
