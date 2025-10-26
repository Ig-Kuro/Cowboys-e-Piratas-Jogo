using UnityEngine;
using System.Collections;

public class NinjaUltimate : Ultimate
{

    public NinjaPersonagem ninja;
    public override void Action()
    {
        ninja.canAttack = false;
        ninja.canUseSkill1 = false;
        ninja.canUseSkill2 = false;
        ninja.movement.canMove = false;
        ninja.canUlt = false;
        ninja.canTakeDamage = false;
        ninja.anim.anim.SetBool("Ulting", true);
        ninja.estado = NinjaPersonagem.State.Ulting;
        ninja.cam2.SetActive(true);   
        ninja.cam1.SetActive(false);
        ninja.clippingMesh.SetActive(true);
        usando = true;
    }

    public override void CmdStartUltimate()
    {
        ninja.cam2.SetActive(false);
        ninja.cam1.SetActive(true);
        ninja.canAttack = true;
        ninja.movement.canMove = true;
        if (isLocalPlayer)
            ninja.clippingMesh.SetActive(false);

        Invoke(nameof(CmdEndUltimate), duration);
        ninja.canTakeDamage = true;
    }

    public override void CmdEndUltimate()
    {
        ninja.canAttack = false;
        ninja.anim.anim.SetTrigger("EndUlt");
        ninja.anim.anim.SetBool("Ulting", false);
        StartCoroutine(EndUltimateAnimation());
    }

    IEnumerator EndUltimateAnimation()
    {
        while (ninja.anim.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }

        ninja.canAttack = true;
        ninja.canUseSkill1 = true;
        ninja.canUseSkill2 = true;
        ninja.canUlt = true;
        currentCharge = 0;
        ninja.estado = NinjaPersonagem.State.Normal;
        usando = false;
    }
}
