using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class PrimeiraSkillCowboy : Skill
{
    public float activationTime, duration;
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
            Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canReload = false;
            usando = true;
            cowboy.estado = Cowboy.State.Lasso;
            cowboy.canUseSkill2 = false;
            cowboy.canUlt = false;
            cowboy.anim.anim.SetTrigger("Laco");
        }
        else Debug.Log("Skill n�o carregada");
    }

    public override void CmdStartSkill()
    {
        CmdSpawnLasso();
        //audioStart.Play();
        Invoke(nameof(CmdEndSkill), duration);
        cowboy.canReload = true;
        currentCooldown = 0;
        cowboy.estado = Cowboy.State.Normal;
        cowboy.canUseSkill2 = true;
        cowboy.canUlt = true;
        currentCooldown = 0;
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnLasso(){
        lassoSpawnado = Instantiate(lassoPrefab, lassoSpawnPoint.position, Quaternion.Euler(lassoSpawnPoint.transform.forward));
        lassoSpawnado.transform.SetParent(lassoSpawnPoint);
        RpcFixLassoPosition(lassoSpawnado);
        NetworkServer.Spawn(lassoSpawnado);
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
        cowboy.playerUI.Skill2StartCD();
        Destroy(lassoSpawnado);
    }

    [Command(requiresAuthority = false)]
    void CmdUnspawnLasso()
    {
        NetworkServer.UnSpawn(lassoSpawnado);
        Destroy(lassoSpawnado);
    }
}
