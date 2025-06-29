using JetBrains.Annotations;
using Mirror;
using UnityEngine;

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
            ci = UIManager.instance.skill1Cooldown;
            ci.cooldownTime = maxCooldown;
            Invoke(nameof(CmdStartSkill), activationTime);
            cowboy.canReload = false;
            cowboy.playerUI.Skill1StartCD();
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
        Destroy(lassoSpawnado);
    }

    [Command(requiresAuthority = false)]
    void CmdUnspawnLasso()
    {
        NetworkServer.UnSpawn(lassoSpawnado);
        Destroy(lassoSpawnado);
    }
}
