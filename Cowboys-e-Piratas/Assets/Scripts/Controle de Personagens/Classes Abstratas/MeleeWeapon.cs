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
    public override void Action()
    {
        if (enemyWeapon)
        {
            Invoke("EvilWeaponSwing", delay);
        }
    }

    private void ResetCombo()
    {
        buffered = false;
        attacking = false;
        currentCombo = 0;
        pirata.anim.AttackPirata(0);
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
            pirata.anim.AttackPirata(currentCombo);
            Invoke("ResetCombo", comboTimer);
        }
        int damageModifier = 1 * currentCombo;
        if (attacking && !buffered)
        {
            if(currentCombo == 1)
            {
                buffered = true;
                CancelInvoke("ResetCombo");
                currentCombo++;
                pirata.anim.AttackPirata(currentCombo);
                Invoke("WeaponSwing", delay);
                Invoke("ResetCombo", comboTimer);
            }
            else if(currentCombo == 2)
            {
                buffered = true;
                CancelInvoke("ResetCombo");
                currentCombo++;
                pirata.anim.AttackPirata(currentCombo);
                Invoke("WeaponSwing", delay * 2);
                Invoke("ResetCombo", comboTimer);
            }
            return;
        }
        attacking = true;
        pirata.anim.AttackPirata(currentCombo);
        Collider[] colider = Physics.OverlapBox(transform.position, attackRange, Quaternion.identity);
        bool enemyHit = false ;
        foreach (Collider col in colider)
        {
            if (col.gameObject.GetComponent<Inimigo>() != null)
            {
                enemyHit = true;
                col.gameObject.GetComponent<Inimigo>().TomarDano(damage * damageModifier);
                if(col.gameObject.GetComponent<Inimigo>().staggerable)
                {
                    col.gameObject.GetComponent<Inimigo>().Push();
                    col.gameObject.GetComponent<Inimigo>().rb.AddForce(transform.parent.forward * pushForce, ForceMode.Impulse);
                }
            }

        }
        Debug.Log(currentCombo);
        //buffered = false;
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
                p.TomarDano(damage);
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(-rb.transform.forward * pushForce, ForceMode.Impulse);
            }
        }
    }
}
