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
        usando = true;
        cowboy.estado = Cowboy.state.lasso;
        cowboy.canUseSkill2 = false;
        cowboy.canUlt = false;
        lassoSpawnado = Instantiate(lassoPrefab, lassoSpawnPoint.position, Quaternion.Euler(lassoSpawnPoint.transform.forward));
        lassoSpawnado.transform.SetParent(lassoSpawnPoint);
        Invoke("EndSkill", duration);
        currentCooldown = 0;
    }

    public override void EndSkill()
    {
        currentCooldown = 0;
        cowboy.estado = Cowboy.state.Normal;
        cowboy.canUseSkill2 = true;
        cowboy.canUlt = true;
        Destroy(lassoSpawnado);
        usando = false;
    }

}
