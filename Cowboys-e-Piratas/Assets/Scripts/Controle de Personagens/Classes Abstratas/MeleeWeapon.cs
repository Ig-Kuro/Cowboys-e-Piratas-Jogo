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
    bool buffered = false;
    bool attacking;
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
    void RecoverAttack()
    {
        canAttack = true;
    }
    public override void Action()
    {
        if (enemyWeapon)
        {
            Invoke(nameof(EvilWeaponSwing), delay);
        }
    }

    private void ResetCombo()
    {
        buffered = false;
        attacking = false;
        currentCombo = 0;
        canAttack = false;
        if (isLocalPlayer)
        {
            pirata.canUseSkill1 = true;
            pirata.canUseSkill2 = true;
            pirata.canUlt = true;
            swingFX.SetActive(false);
        }
        Invoke(nameof(RecoverAttack), 0.5f);
    }

    public void WeaponSwingPirata()
    {
        if (!canAttack) return;

        if (currentCombo == 0 && !attacking)
        {
            currentCombo = 1;
            attacking = true;
            //swingDir = Vector3.right;
            pirata.anim.SetAttack1Pirata();
            pirata.canUseSkill1 = false;
            pirata.canUseSkill2 = false;
            pirata.canUlt = false;

            Invoke(nameof(ResetCombo), comboTimer);
            //Invoke(nameof(WeaponSwingPirata), delay);
            //CmdPerformAttack(transform.position, currentCombo, swingDir);
            swingFX.SetActive(true);
            return;
        }
        else if (attacking && !buffered)
        {
            if (currentCombo == 1)
            {
                buffered = true;
                currentCombo = 2;
                //swingDir = -Vector3.right;
                pirata.anim.SetAttack2Pirata();
                pirata.canUseSkill1 = false;
                pirata.canUseSkill2 = false;
                pirata.canUlt = false;

                CancelInvoke(nameof(ResetCombo));
                //Invoke(nameof(WeaponSwingPirata), delay);
                Invoke(nameof(ResetCombo), comboTimer);
                //CmdPerformAttack(transform.position, currentCombo, swingDir);
                return;
            }
            else if (currentCombo == 2)
            {
                buffered = true;
                currentCombo = 3;
               // swingDir = Vector3.forward;
                pirata.anim.SetAttack3Pirata();
                pirata.canUseSkill1 = false;
                pirata.canUseSkill2 = false;
                pirata.canUlt = false;

                //CancelInvoke(nameof(ResetCombo));
                //Invoke(nameof(WeaponSwingPirata), delay * 2);
                //Invoke(nameof(ResetCombo), comboTimer * 2);
                //CmdPerformAttack(transform.position, currentCombo, swingDir);
                return;
            }
        }

        buffered = false;
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

    public void WeaponSwing()
    {
        /*GameObject hitbox = Instantiate(hitBoxVizualizer, transform.position, transform.rotation);
        Destroy(hitbox, 5f);
        hitbox.transform.localScale = new Vector3(attackRange.x, attackRange.y, attackRange.z *2);*/

        //swingAudio.Play();
        if (!canAttack)
            return;
        if (currentCombo == 0 && !attacking)
        {
            currentCombo = 1;
            Invoke(nameof(ResetCombo), comboTimer);
            Invoke(nameof(WeaponSwing), delay);
            swingDir = Vector3.right;
            return;
        }
        else if (attacking && !buffered)
        {
            if (currentCombo == 1)
            {
                buffered = true;
                CancelInvoke(nameof(ResetCombo));
                currentCombo++;
                Invoke(nameof(WeaponSwing), delay);
                Invoke(nameof(ResetCombo), comboTimer);
                swingDir = -Vector3.right;
                return;
            }
            else if (currentCombo == 2)
            {
                buffered = true;
                CancelInvoke(nameof(ResetCombo));
                currentCombo++;
                Invoke(nameof(WeaponSwing), delay);
                Invoke(nameof(ResetCombo), comboTimer);
                swingDir = Vector3.forward;
                return;
            }
        }
        int damageModifier = 1 * currentCombo;
        attacking = true;
        Collider[] colider = Physics.OverlapBox(transform.position, attackRange, Quaternion.identity);
        bool enemyHit = false;
        foreach (Collider col in colider)
        {
            if (col.gameObject.TryGetComponent<Inimigo>(out var enemy))
            {
                enemyHit = true;
                enemy.damage.damageType = damageInfo.damageType;
                enemy.CalculateDamageDir(swingDir);
                enemy.TakeDamage(damage * damageModifier);

                if (enemy.canbeStaggered)
                {
                    enemy.Push();
                    enemy.rb.AddForce(transform.parent.forward * pushForce, ForceMode.Impulse);
                }
            }

        }
        Debug.Log(currentCombo);
        if (enemyHit)
        {
            // hitEnemyAudio.Play();
        }
        buffered = false;
    }

    public void EvilWeaponSwing()
    {
        PerformEnemyAttack(transform.position, swingDir);
        
        if (!canAttack || attacking) return;

        attacking = true;
        swingDir = Vector3.right; // Ou outra direção, se necessário
        Invoke(nameof(ResetCombo), comboTimer);
    }

    [ClientRpc]
    void PerformEnemyAttack(Vector3 position, Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(position, attackRange, Quaternion.identity);

        HashSet<Personagem> damagedPlayers = new();

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out Personagem personagem) && !damagedPlayers.Contains(personagem))
            {
                
                personagem.TakeDamage(damage);
                damagedPlayers.Add(personagem);
            }
        }
    }
}
