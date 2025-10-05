using UnityEngine;

public class UltimateViking : Ultimate
{
    public VikingPersonagem viking;
    float defaultSpeed;
    int defaultDano;
    float defaultArmor;
    public int damageMultiplier;
    private void Start()
    {
        defaultSpeed = viking.speed;
        defaultArmor = viking.armor;
        defaultDano = viking.damgeMultiplier;
    }
    public override void Action()
    {
        viking.canAttack = false;
        viking.canUseSkill1 = false;    
        viking.canUseSkill2 = false;
        usando = true;
        viking.state = VikingPersonagem.Estado.Ultando;
    }

    public override void CmdStartUltimate()
    {
        viking.speed = defaultSpeed * 2f;
        viking.armor = defaultArmor + 10;
        viking.damgeMultiplier = defaultDano * damageMultiplier;
        viking.canAttack = true;
        viking.currentHp /= 2;
        Debug.Log("Raaaaagh");
        Invoke(nameof(CmdEndUltimate), duration);
    }

    public override void CmdEndUltimate()
    {
        viking.speed = defaultSpeed;
        viking.armor = defaultArmor;
        viking.damgeMultiplier = defaultDano;
        Debug.Log("Acalmei");
        viking.state = VikingPersonagem.Estado.Normal;

    }
}
