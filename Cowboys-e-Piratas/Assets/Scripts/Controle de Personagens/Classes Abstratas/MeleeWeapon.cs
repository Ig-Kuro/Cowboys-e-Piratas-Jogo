using UnityEngine;
using Mirror;
using Mirror.BouncyCastle.Asn1.X509;
using Unity.PlasticSCM.Editor.WebApi;

public class MeleeWeapon : Arma
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

    private void Start()
    {
        damageInfo = new DamageInfo();
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
        pirata.canUseSkill1 = true;
        pirata.canUseSkill2 = true;
        pirata.canUlt = true;
        Invoke(nameof(RecoverAttack), 0.5f);
    }


    public void WeaponSwingPirata()
    {
        if (!canAttack)
            return;
        if (currentCombo == 0 && !attacking)
        {
            currentCombo = 1;
            Invoke("ResetCombo", comboTimer);
            Invoke("WeaponSwingPirata", delay);
            pirata.anim.SetAttack1Pirata();
            swingDir = Vector3.right;
            attacking = true;
            pirata.canUseSkill1 = false;
            pirata.canUseSkill2 = false;
            pirata.canUlt = false;
            return;
        }
        else if (attacking && !buffered)
        {
            if (currentCombo == 1)
            {
                buffered = true;
                CancelInvoke("ResetCombo");
                currentCombo++;
                Invoke("WeaponSwingPirata", delay);
                Invoke("ResetCombo", comboTimer);
                pirata.canUseSkill1 = false;
                pirata.canUseSkill2 = false;
                pirata.canUlt = false;
                pirata.anim.SetAttack2Pirata();
                swingDir = -Vector3.right;
                return;
            }
            else if (currentCombo == 2)
            {
                buffered = true;
                CancelInvoke("ResetCombo");
                currentCombo++;
                Invoke("WeaponSwingPirata", delay * 2);
                Invoke("ResetCombo", comboTimer *2 );
                pirata.canUseSkill1 = false;
                pirata.canUseSkill2 = false;
                pirata.canUlt = false;
                pirata.anim.SetAttack3Pirata();
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
            if (col.gameObject.GetComponent<Inimigo>() != null)
            {
                enemyHit = true;
                col.gameObject.GetComponent<Inimigo>().damage.damageType = damageInfo.damageType;
                col.gameObject.GetComponent<Inimigo>().CalculateDamageDir(swingDir);
                col.gameObject.GetComponent<Inimigo>().TakeDamage(damage * damageModifier);

                if (col.gameObject.GetComponent<Inimigo>().staggerable)
                {
                    col.gameObject.GetComponent<Inimigo>().Push();
                    col.gameObject.GetComponent<Inimigo>().rb.AddForce(transform.parent.forward * pushForce, ForceMode.Impulse);
                }
            }

        }
        if (enemyHit)
        {
            // hitEnemyAudio.Play();
        }
        buffered = false;
    }
    public void WeaponSwing()
    {
        /*GameObject hitbox = Instantiate(hitBoxVizualizer, transform.position, transform.rotation);
        Destroy(hitbox, 5f);
        hitbox.transform.localScale = new Vector3(attackRange.x, attackRange.y, attackRange.z *2);*/

        //swingAudio.Play();
        if (!canAttack)
            return;
        if(currentCombo == 0 && !attacking)
        {
            currentCombo = 1;
            Invoke(nameof(ResetCombo), comboTimer);
            Invoke(nameof(WeaponSwing), delay);
            swingDir = Vector3.right;
            return;
        }
        else if (attacking && !buffered)
        {
            if(currentCombo == 1)
            {
                buffered = true;
                CancelInvoke(nameof(ResetCombo));
                currentCombo++;
                Invoke(nameof(WeaponSwing), delay);
                Invoke(nameof(ResetCombo), comboTimer);
                swingDir = -Vector3.right;
                return;
            }
            else if(currentCombo == 2)
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
        bool enemyHit = false ;
        foreach (Collider col in colider)
        {
            if (col.gameObject.TryGetComponent<Inimigo>(out var enemy))
            {
                enemyHit = true;
                enemy.damage.damageType = damageInfo.damageType;
                enemy.CalculateDamageDir(swingDir);
                enemy.TakeDamage(damage * damageModifier);

                if(enemy.staggerable)
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
        /*GameObject hitbox = Instantiate(hitBoxVizualizer, transform.position, transform.rotation);
        Destroy(hitbox, 5f);
        hitbox.transform.localScale = new Vector3(attackRange.x, attackRange.y, attackRange.z * 2);*/
        Collider[] colider = Physics.OverlapBox(transform.position, attackRange, Quaternion.identity);
        foreach (Collider col in colider)
        {
            if (col.gameObject.GetComponent<Personagem>() != null)
            {
                Personagem p = col.gameObject.GetComponent< Personagem>();
                p.TakeDamage(damage);
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(-rb.transform.forward * pushForce, ForceMode.Impulse);
            }
        }
    }
}
