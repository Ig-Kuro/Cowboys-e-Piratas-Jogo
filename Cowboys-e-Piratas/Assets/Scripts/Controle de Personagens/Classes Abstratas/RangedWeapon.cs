using System.Collections;
using Mirror;
using UnityEngine;

public class RangedWeapon : BaseWeapon
{
    [Header("Valores para armas de fogo")]
    public float bulletsPerShot;
    public float recoil;
    public float reloadTime;
    public float spread;
    public int maxAmmo;
    public bool canHeadShot = true;
    [SyncVar]public bool reloading, canShoot;
    public int currentAmmo;
    public float pushForce;
    public AudioSource shootNoise, reloadNoise, emptyClipNoise;

    [Header("Se For projetil")]
    public bool projectile = false;
    public Vector3 projectileTarget;
    public ProjectileBullet bullet;

    [Header("Outros Componentes")]
    public NoiseCamera noiseCam;
    public Transform bulletPoint;
    public TrailRenderer trail;
    public float delay;
    public GameObject shootDir;
    public GameObject enemyTarget;

    [Header("VFXs")]
    public GameObject shootFX, hitFX, bloodFX;



    [Header("Para Objetos Arremessaveis")]
    public bool throwable = false;
    public GameObject weapon;


    RaycastHit raycast;
    int bulletsShot;

    private void Start()
    {
        reloading = false;
        canShoot = true;
        currentAmmo = maxAmmo;
        player?.playerUI?.UpdateAmmo(this);
    }

    public override void Action()
    {
        if (!canShoot || reloading) return;
        if (currentAmmo <= 0)
        {
            //emptyClipNoise?.Play();
            return;
        }

        //TODO
        // Feedback imediato (prediction local)
        //LocalShootFX();

        // Envia requisição para o servidor validar/acertar
        CmdRequestShoot();
    }

    private void LocalShootFX()
    {
        // VFX + SFX local
        InstantiateFX(shootFX, bulletPoint.position, shootDir.transform.rotation);
        //shootNoise?.Play();

        // Consome 1 bala localmente (prediction)
        currentAmmo--;
        player?.playerUI?.UpdateAmmo(this);

        // Bloqueia ataque até próximo tiro
        canShoot = false;
        Invoke(nameof(ResetAttack), attackRate);
    }
    
    [Command]
    private void CmdRequestShoot(NetworkConnectionToClient sender = null)
    {
        if (currentAmmo <= 0) return;

        bulletsShot = 0;

        if (projectile)
            StartCoroutine(ShootProjectileRoutine());
        else
            StartCoroutine(ShootHitscanRoutine());
    }

    #region Rotinas de disparo
    private IEnumerator ShootHitscanRoutine(bool useDelay = true)
    {
        yield return new WaitForSeconds(useDelay ? delay : 0f);

        DoShootHitScan();

        FinishShoot(new Vector3(recoil, recoil * 2, 0) * Time.deltaTime,
            () => StartCoroutine(ShootHitscanRoutine(false)));
    }

    private IEnumerator ShootProjectileRoutine()
    {
        yield return new WaitForSeconds(delay);

        DoShootProjectile();

        FinishShoot(new Vector3(recoil / 2, recoil, 0) * Time.deltaTime, 
            () => StartCoroutine(ShootProjectileRoutine()));
    }
    #endregion

    [Server]
    private void DoShootHitScan()
    {
        RpcPlayShootFX(); // manda efeitos pros clientes

        Vector3 direction = CalculateDirection(spread);
        if (Physics.Raycast(shootDir.transform.position, direction, out raycast, reach))
        {
            CreateTrail(raycast);
            ProcessHit(raycast, direction);
        }
    }


    #region Arremesaveis
    void CmdThrowThrowable()
    {
        canShoot = false;
        Vector3 direction = CalculateDirection(spread);
        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            ShootProjetil(raycast.point);
            weapon.SetActive(false);
        }
        FinishShoot(new Vector3(recoil / 2, recoil, 0) * Time.deltaTime, () => StartCoroutine(ShootProjectileRoutine()));
    }


    public void ResetThrowable()
    {
        weapon.SetActive(true);
    }

    public void DestroyThrowable()
    {
        if(weapon.activeInHierarchy )
        {
            weapon.SetActive(false);
        }
        else
        {
            CancelInvoke(nameof(ResetThrowable));
        }
    }
    #endregion

    [Server]
    private void DoShootProjectile()
    {
        Vector3 direction = CalculateDirection(spread);
        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            ShootProjetil(raycast.point);
        }
    }

    #region Funções Comuns
    Vector3 CalculateDirection(float spreadValue)
    {
        canShoot = false;
        float spreadX = Random.Range(-spreadValue, spreadValue);
        float spreadY = Random.Range(-spreadValue, spreadValue);
        Vector3 dir = (projectile ? bulletPoint.forward : shootDir.transform.forward) + new Vector3(spreadX, spreadY, 0);
        Debug.DrawRay(bulletPoint.position, dir, Color.red, 2f);
        return dir.normalized;
    }

    void CreateTrail(RaycastHit hit)
    {
        TrailRenderer bulletTrail = Instantiate(trail, bulletPoint.position, Quaternion.Euler(bulletPoint.forward));
        StartCoroutine(GenerateTrail(bulletTrail, hit));
        NetworkServer.Spawn(bulletTrail.gameObject);
    }

    void ProcessHit(RaycastHit hit, Vector3 direction)
    {
        if (hit.collider.CompareTag("Inimigo"))
        {
            Inimigo enemy = hit.collider.GetComponentInParent<Inimigo>();
            if (enemy != null)
            {
                enemy.CalculateDamageDir(hit.point);
                bool headshot = hit.collider == enemy.headshotCollider && canHeadShot;
                ApplyDamage(enemy, direction, hit.point, headshot);
            }
        }
        else
        {
            GameObject gO = Instantiate(hitFX, hit.point, hit.transform.localRotation);
            Destroy(gO, 0.2f);
        }
    }

    void ApplyDamage(Inimigo enemy, Vector3 direction, Vector3 hitPoint, bool headshot)
    {
        if (enemy.canbeStaggered)
        {
            enemy.Push();
            enemy.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
        }

        if (headshot)
        {
            InstantiateFX(bloodFX, hitPoint, enemy.transform.rotation);
            enemy.TakeDamage(damage * 2);
            ultimate.AddUltPoints(damage/5);
        }
        else
        {
            enemy.TakeDamage(damage);
            ultimate.AddUltPoints(damage/10);
        }
    }

    [ClientRpc]
    private void RpcPlayShootFX()
    {
        // Reproduz efeitos só nos OUTROS clients
        if (isOwned) return; 
        InstantiateFX(shootFX, bulletPoint.position, shootDir.transform.rotation);
        //shootNoise?.Play();
    }

    [ClientRpc]
    private void RpcUpdateAmmo(int ammo)
    {
        currentAmmo = ammo;
        player?.playerUI?.UpdateAmmo(this);
    }

    [ClientRpc]
    private void RpcPlayNoise(Vector3 noise)
    {
        noiseCam?.PlayNoise(noise);
    }

    void InstantiateFX(GameObject fxPrefab, Vector3 pos, Quaternion rot, float destroyTime = 0.2f)
    {
        GameObject fx = Instantiate(fxPrefab, pos, rot);
        Destroy(fx, destroyTime);
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
            RpcPlayNoise(noise);

        if (bulletsShot < bulletsPerShot)
        {
            nextAction();
        }
        else
        {
            bulletsShot = 0;
            currentAmmo--;
            RpcUpdateAmmo(currentAmmo);

            canShoot = false;
            //player?.playerUI?.UpdateAmmo(this);
            /*if(throwable)
                Invoke(nameof(ResetThrowable), attackRate/10);*/
            
            Invoke(nameof(ResetAttack), attackRate);

        }
    }

    void ResetAttack() => canShoot = true;
    #endregion

    #region Recarregar
    public void Reload()
    {
        // Se já estamos no servidor, inicia direto. Se cliente, pede ao servidor.
        if (isServer)
        {
            StartReloadOnServer();
        }
        else
        {
            CmdRequestReload();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestReload(NetworkConnectionToClient sender = null)
    {
        // executa no servidor
        StartReloadOnServer();
    }

    // função que roda sempre no servidor
    [Server]
    private void StartReloadOnServer()
    {
        if (currentAmmo >= maxAmmo || reloading) return;

        reloading = true;
        canShoot = false;

        // agenda finalização da recarga no servidor
        Invoke(nameof(FinishReloadingServer), reloadTime);
    }

    [Server]
    private void FinishReloadingServer()
    {
        currentAmmo = maxAmmo;
        reloading = false;

        // informa clientes sobre a nova munição (já existia RpcUpdateAmmo)
        RpcUpdateAmmo(currentAmmo);

        // agenda permitir atirar novamente (servidor)
        Invoke(nameof(ResetAttack), attackRate);
    }
    #endregion

    IEnumerator GenerateTrail(TrailRenderer t, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = t.transform.position;
        while (time < 1)
        {
            t.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / t.time;
            yield return null;
        }
        t.transform.position = hit.point;
        Destroy(t.gameObject, t.time);
    }

    [Server]
    public IEnumerator ShootEnemyProjectile()
    {
        yield return new WaitForSeconds(delay);
        if (canShoot)
        {
            ShootProjetil(projectileTarget);
            FinishShoot(Vector3.zero, () => StartCoroutine(ShootEnemyProjectile()));
        }
    }
}
