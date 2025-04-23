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
            CmdStartSkill();
            cowboy.canReload = false;
        }
        else Debug.Log("Skill n�o carregada");
    }

    public override void CmdStartSkill()
    {
        usando = true;
        cowboy.estado = Cowboy.state.lasso;
        cowboy.canUseSkill2 = false;
        cowboy.canUlt = false;
        CmdSpawnLasso();
        Invoke(nameof(CmdEndSkill), duration);
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
        cowboy.canReload = true;
        currentCooldown = 0;
        cowboy.estado = Cowboy.state.Normal;
        cowboy.canUseSkill2 = true;
        cowboy.canUlt = true;
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
