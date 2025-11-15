using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] InimigoPerto enemy;
    public void Explosion()
    {
        enemy.DeathAction();
    }
}
