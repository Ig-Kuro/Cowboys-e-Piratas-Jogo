using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] PlayerMeleeWeapon weapon;

    public void OnAttackHit()
    {
        weapon.OnAttackHit();
    }

    public void OnAttackEnd()
    {
        weapon.OnAttackEnd();
    }
}
