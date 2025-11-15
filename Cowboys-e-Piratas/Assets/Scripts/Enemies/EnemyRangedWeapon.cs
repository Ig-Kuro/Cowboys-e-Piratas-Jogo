using System.Collections;
using Mirror;
using UnityEngine;

public class EnemyRangedWeapon : BaseWeapon
{
    [Header("Valores para armas de fogo")]
    public float bulletsPerShot;
    public float recoil;
    [SyncVar]public bool reloading, canShoot;
    public float pushForce;

    [Header("Se For projetil")]
    public bool projectile = false;
    public Vector3 projectileTarget;
    public ProjectileBullet bullet;

    [Header("Outros Componentes")]
    public Transform bulletPoint;
    public float delay;
    public GameObject shootDir;

    RaycastHit raycast;
    int bulletsShot;

    private void Start()
    {
        reloading = false;
        canShoot = true;
    }

    public override void Action()
    {
        if (!canShoot || reloading) return;
        CmdRequestShoot();
    }
    
    [Command]
    private void CmdRequestShoot(NetworkConnectionToClient sender = null)
    {
        bulletsShot = 0;

        StartCoroutine(ShootProjectileRoutine());
    }

    #region Rotinas de disparo
    private IEnumerator ShootProjectileRoutine()
    {
        yield return new WaitForSeconds(delay);

        DoShootProjectile();

        FinishShoot(new Vector3(recoil / 2, recoil, 0) * Time.deltaTime, 
            () => StartCoroutine(ShootProjectileRoutine()));
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
    Vector3 CalculateDirection()
    {
        canShoot = false;
        Vector3 dir = projectile ? bulletPoint.forward : shootDir.transform.forward;
        Debug.DrawRay(bulletPoint.position, dir, Color.red, 2f);
        return dir.normalized;
    }

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

    void FinishShoot(Vector3 noise, System.Action nextAction)
    {
        bulletsShot++;
        if (noise != Vector3.zero)

        if (bulletsShot < bulletsPerShot)
        {
            nextAction();
        }
        else
        {
            bulletsShot = 0;

            canShoot = false;
            
            Invoke(nameof(ResetAttack), attackRate);
        }
    }

    void ResetAttack() => canShoot = true;

    #endregion
}
