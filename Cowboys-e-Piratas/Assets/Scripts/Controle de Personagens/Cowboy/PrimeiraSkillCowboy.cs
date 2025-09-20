using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class PrimeiraSkillCowboy : Skill
{
    public GameObject lassoPrefab;
    public Cowboy cowboy;
    GameObject lassoSpawnado;

    //sincronizar posição disso
    public Transform lassoSpawnPoint;

    public override void Action()
    {
        if (FinishedCooldown())
        {
            ci.cooldownImage.fillAmount = 0;
            ci.inCooldown = false;
            cowboy.canReload = false;
            usando = true;
            cowboy.estado = Cowboy.State.Lasso;
            cowboy.canUseSkill2 = false;
            cowboy.canUlt = false;
            cowboy.canAttack = false;
            cowboy.anim.anim.SetTrigger("Laco");
            Invoke(nameof(CmdStartSkill), activationTime);
        }
        else Debug.Log("Skill não carregada");
    }

    public override void CmdStartSkill()
    {
        CmdSpawnLasso();
        //audioStart.Play();
        Invoke(nameof(CmdEndSkill), duration);
        Invoke(nameof(ResetStates), duration/2);
        currentCooldown = 0;
        currentCooldown = 0;
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnLasso(){
        lassoSpawnado = Instantiate(lassoPrefab, lassoSpawnPoint.position, Quaternion.Euler(lassoSpawnPoint.transform.forward));
        lassoSpawnado.transform.SetParent(lassoSpawnPoint);
        NetworkServer.Spawn(lassoSpawnado);
        RpcFixLassoPosition(lassoSpawnado);
    }

    [ClientRpc]
    void RpcFixLassoPosition(GameObject lasso)
    {
        if (lasso != null)
        {
            lasso.transform.localPosition = Vector3.zero;
        }
    }

    public override void CmdEndSkill()
    {
        CmdUnspawnLasso();
        usando = false;
        ci.inCooldown = true;
        cowboy.playerUI.Skill1StartCD();
        Destroy(lassoSpawnado);
    }

    private void ResetStates()
    {
        cowboy.RestartReturnToIdle();
        cowboy.estado = Cowboy.State.Normal;
    }

    [Command(requiresAuthority = false)]
    void CmdUnspawnLasso()
    {
        NetworkServer.UnSpawn(lassoSpawnado);
        Destroy(lassoSpawnado);
    }
}
