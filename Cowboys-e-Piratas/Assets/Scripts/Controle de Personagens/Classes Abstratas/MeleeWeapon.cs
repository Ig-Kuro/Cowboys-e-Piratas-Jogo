using UnityEngine;
using Mirror;
using Mirror.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;

public class MeleeWeapon : BaseWeapon
{
    public float delay;
    public bool canAttack = true;
    public float pushForce;
    public bool right = true;
    public bool enemyWeapon;
    public Vector3 attackRange;
    public Transform leftPoint, rightPoint;
    public GameObject espada;
    public GameObject hitBoxVizualizer;
    public AudioSource swingAudio, hitEnemyAudio, hitObjectAudio;
    public int currentCombo = 0;
    public float comboTimer;
    public Pirata pirata;
    DamageInfo damageInfo;
    Vector3 swingDir;
    public GameObject swingFX;
    public GameObject bloodFX;

    [HideInInspector]
    public bool killedEnemy;
    public int enemiesKilled;

    private void Start()
    {
        damageInfo = ScriptableObject.CreateInstance<DamageInfo>();
        damageInfo.damageType = DamageInfo.DamageType.Melee;
    }
    public override void Action()
    {
        
    }

    [Command(requiresAuthority = false)]
    public void CmdPerformAttack(Vector3 position, int combo, Vector3 direction)
    {
        int damageModifier = combo;
        Collider[] coliders = Physics.OverlapBox(position, attackRange, Quaternion.identity);
        killedEnemy = false;
        enemiesKilled = 0;

        foreach (Collider col in coliders)
        {
            if (col.TryGetComponent(out Inimigo enemy))
            {
                enemy.damage.damageType = damageInfo.damageType;
                GameObject blood = Instantiate(bloodFX, col.transform.position, enemy.transform.rotation);
                int dealtDamage = damage * damageModifier;
                enemy.CalculateDamageDir(direction);
                enemy.TakeDamage(dealtDamage);

                if(enemy.health <= dealtDamage)
                {
                    killedEnemy = true;
                    enemiesKilled++;
                }

                if (enemy.canbeStaggered)
                {
                    enemy.Push();
                    enemy.rb.AddForce(direction.normalized * pushForce, ForceMode.Impulse);
                }
            }
        }
        //RpcPlayHitEffect();
        Debug.Log("Ataquei");
    }

    [ClientRpc]
    void RpcPlayHitEffect()
    {
        // swingAudio.Play();
        // hitEnemyAudio.Play();
    }
}
