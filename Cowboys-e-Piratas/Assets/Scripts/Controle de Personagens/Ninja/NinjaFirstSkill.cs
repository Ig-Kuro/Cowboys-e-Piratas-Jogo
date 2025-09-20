using Mirror;
using Mirror.BouncyCastle.Asn1.BC;
using UnityEngine;

public class NinjaFirstSkill : Skill
{

    public NinjaPersonagem ninja;
    public override void Action()
    {
        if (FinishedCooldown())
        {
            TargetStartSkill1CD(connectionToClient);
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
            ninja.estado = NinjaPersonagem.State.Trocando;
            Debug.Log("Skll1");


            ninja.canAttack = false;
            ninja.canUseSkill2 = false;
        }
    }


    public override void CmdStartSkill()
    {
        if(ninja.armaAtual == ninja.shuriken)
        {
            ninja.shuriken.DestroyThrowable();
            ninja.shuriken.GetComponent<RangedWeapon>().enabled = false;
            ninja.kunai.GetComponent<RangedWeapon>().enabled = true;
            ninja.kunai.ResetThrowable();
            ninja.armaAtual = ninja.kunai;
        }

        else if (ninja.armaAtual == ninja.kunai)
        {
            ninja.kunai.DestroyThrowable();
            ninja.kunai.GetComponent<RangedWeapon>().enabled = false;
            ninja.shuriken.GetComponent<RangedWeapon>().enabled = true;
            ninja.shuriken.ResetThrowable();
            ninja.armaAtual = ninja.shuriken;
        }
        ninja.canAttack = true;
        ninja.canUseSkill2 = true;
        currentCooldown = 0;
        ninja.estado = NinjaPersonagem.State.Normal;
        Debug.Log(ninja.armaAtual);
    }


    void TargetStartSkill1CD(NetworkConnection target)
    {
        if (ninja.playerUI != null)
        {

        }
    }
}
