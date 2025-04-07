using UnityEngine;
using Mirror;

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
    public override void Action()
    {
        if (!canAttack)
        {
            return;
        }
        Invoke("ResetAttack", attackRate);
        if (!enemyWeapon)
        {
            Invoke("WeaponSwing", delay);
        }
        else
        {
            Invoke("EvilWeaponSwing", delay);
        }
    }

    public void ResetAttack()
    {
        canAttack = true;
    }

    public void WeaponSwing()
    {
        /*GameObject hitbox = Instantiate(hitBoxVizualizer, transform.position, transform.rotation);
        Destroy(hitbox, 5f);
        hitbox.transform.localScale = new Vector3(attackRange.x, attackRange.y, attackRange.z *2);*/
       
        if (right)
        {
            right = false;
        }
        else if (!right)
        {
            right = true;
        }
        Collider[] colider = Physics.OverlapBox(transform.position, attackRange, Quaternion.identity);
        foreach (Collider col in colider)
        {
            if (col.gameObject.GetComponent<Inimigo>() != null && col.gameObject.GetComponent<Inimigo>().staggerable)
            {
                col.gameObject.GetComponent<Inimigo>().Push();
                col.gameObject.GetComponent<Inimigo>().TomarDano(damage);
                col.gameObject.GetComponent<Inimigo>().rb.AddForce(transform.parent.forward * pushForce, ForceMode.Impulse);
            }

        }
    }

    public bool Swung(bool happened)
    {
        return happened;
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
