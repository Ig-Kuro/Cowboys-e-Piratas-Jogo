using UnityEngine;

public class InimigoVoador : Inimigo
{
    public MeleeWeapon weapon;

    public override void PerformAttack()
    {
        if (recovering) return;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out RaycastHit ray, attackRange))
        {
            if (ray.collider.CompareTag("Player"))
            {

                anim.SetTrigger("Attack");
                weapon.Action();
                recovering = true;
                Invoke(nameof(Recover), weapon.attackRate + weapon.delay);
            }
        }
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        anim.SetTrigger("Damage");
    }
}
