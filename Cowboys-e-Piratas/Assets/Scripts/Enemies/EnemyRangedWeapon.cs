using System.Collections;
using Mirror;
using UnityEngine;

public class EnemyRangedWeapon : BaseWeapon
{
    [Header("Valores para armas de fogo")]
    public float bulletsPerShot;
    public float recoil;
    [SyncVar]public bool canShoot;
    public float pushForce;

    [Header("Se For projetil")]
    public bool projectile = false;
    public Vector3 projectileTarget;
    public ProjectileBullet bullet;

    [Header("Outros Componentes")]
    public Transform bulletPoint;
    public float delay;

    RaycastHit raycast;

    private void Start()
    {
        if (!isServer) return;
        canShoot = true;
    }

    public override void Action()
    {
        if (!isServer) return;

        if (!canShoot) return;
        RequestShoot();
    }
    
    private void RequestShoot()
    {
        StartCoroutine(ShootProjectileRoutine());
    }

    #region Rotinas de disparo
    private IEnumerator ShootProjectileRoutine()
    {
        yield return new WaitForSeconds(delay);

        DoShootProjectile();

        FinishShoot();
    }
    #endregion

    [Server]
    private void DoShootProjectile()
    {
        Vector3 direction = CalculateDirection();
        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            ShootProjetil(raycast.point);
        }
    }

    #region Funções Comuns
    [Server]
    Vector3 CalculateDirection()
    {
        canShoot = false;
        Vector3 dir = bulletPoint.forward;
        Debug.DrawRay(bulletPoint.position, dir, Color.red, 2f);
        return dir.normalized;
    }

    [Server]
    void ShootProjetil(Vector3 target)
    {
        ProjectileBullet projectile = Instantiate(bullet, bulletPoint.position, Quaternion.Euler(bulletPoint.forward));
        projectile.ult = ultimate;
        projectile.target = target;
        projectile.damage = damage;
        projectile.pushForce = pushForce;
        NetworkServer.Spawn(projectile.gameObject);
        projectile.Move(gameObject);
    }

    [Server]
    void FinishShoot()
    {
        canShoot = false;
        
        Invoke(nameof(ResetAttack), attackRate);
    }

    [Server]
    void ResetAttack() => canShoot = true;

    #endregion
}
