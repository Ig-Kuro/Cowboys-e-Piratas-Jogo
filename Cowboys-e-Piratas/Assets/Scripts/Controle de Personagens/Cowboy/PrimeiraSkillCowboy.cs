using JetBrains.Annotations;
using UnityEngine;

public class PrimeiraSkillCowboy : Skill
{
    public float activationTime, duration;
    public GameObject lassoPrefab;
    public Cowboy cowboy;
    GameObject lassoSpawnado;
    public Transform lassoSpawnPoint;
    public override void Action()
    {

        if (FinishedCooldown())
        {
            StartSkill();
        }
        else Debug.Log("Skill não carregada");
    }
    public override void StartSkill()
    {
        cowboy.estado = Cowboy.state.lasso;
        cowboy.canUseSkill2 = false;
        lassoSpawnado = Instantiate(lassoPrefab, lassoSpawnPoint.position, Quaternion.Euler(lassoSpawnPoint.transform.forward));
        lassoSpawnado.transform.SetParent(lassoSpawnPoint);
        Invoke("EndSkill", duration);
        currentCooldown = 0;
    }

    public override void EndSkill()
    {
        currentCooldown = 0;
        cowboy.estado = Cowboy.state.Normal;
        Destroy(lassoSpawnado);
    }

}
