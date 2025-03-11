using JetBrains.Annotations;
using UnityEngine;

public class PrimeiraSkillCowboy : Skill
{
    public float activationTime, duration;
    public GameObject lassoPrefab;
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

        lassoSpawnado = Instantiate(lassoPrefab, lassoSpawnPoint.position, Quaternion.Euler(lassoSpawnPoint.transform.forward));
        lassoSpawnado.transform.SetParent(lassoSpawnPoint);
        Invoke("EndSkill", duration);
    }

    public override void EndSkill()
    {
        Destroy(lassoSpawnado);
    }

}
