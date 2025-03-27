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
            Invoke(nameof(CmdStartSkill), activationTime);
        }
        else if(FinishedCooldown() && cowboy.estado == Cowboy.state.rifle)
        {
            CmdEndSkill();
        }
        else Debug.Log("Skill n�o carregada");
    }

    public override void CmdStartSkill()
    {
        Debug.Log("Start rifle");
        cowboy.canAttack = false;
        cowboy.canReload = false;
        cowboy.primeiraPistola.gameObject.SetActive(false);
        usando = true;
        cowboy.estado = Cowboy.state.rifle;
        cowboy.rifle.currentAmmo = cowboy.rifle.maxAmmo;
        cowboy.rifle.gameObject.SetActive(true);
        cowboy.armaAtual = cowboy.rifle;
        cowboy.canUseSkill1 = false;
        Invoke(nameof(CmdEndSkill), duration);
    }

    public override void CmdEndSkill()
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
