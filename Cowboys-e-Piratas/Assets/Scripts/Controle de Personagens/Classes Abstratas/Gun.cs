using System.Collections;
using Mirror;
using UnityEngine;

public class Gun : Arma
{
    [Header("Valores para armas de fogo")]
    public float bulletsPerShot;
    public float recoil;
    public float reloadTime;
    public float spread;
    public int maxAmmo;
    public bool canHeadShot = true;
    public bool reloading, canShoot;
    public int currentAmmo;
    int bulletsShot;
    public float pushForce;
    public float bufferTimer;
    public AudioSource shootNoise, reloadNoise, emptyClipNoise;

    [Header("Se For projetil")]
    public bool projectile = false;
    public Vector3 projectileTarget;
    public ProjectileBullet bullet;

    [Header("Outros Componentes")]
    public NoiseCamera noiseCam;
    public Transform bulletPoint;
    RaycastHit raycast;
    public TrailRenderer trail;
    public bool bufferedShot, bufferedReload;
    public float delay;
    public GameObject shootDir;
    public GameObject enemyTarget;

    private void Start()
    {
        reloading = false;
        canShoot = true;
        currentAmmo = maxAmmo;
        if (player != null)
        {
            player.playerUI?.UpdateAmmo(this);
        }
    }

    public override void Action()
    {
        if (canShoot && !reloading && currentAmmo > 0)
        {
            bulletsShot = 0;
            if (!projectile)
            {
                Invoke(nameof(CmdShootHitScan), delay);
            }
            else
            {
                CmdShootProjectile();
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdShootHitScan()
    {
        canShoot = false;
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        Vector3 direction = shootDir.transform.forward + new Vector3(spreadX, spreadY, 0);
        direction.Normalize();

        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            TrailRenderer bulletTrail = Instantiate(trail, bulletPoint.transform.position, Quaternion.Euler(bulletPoint.forward));
            StartCoroutine(GenerateTrail(bulletTrail, raycast));
            NetworkServer.Spawn(bulletTrail.gameObject);
            //shootNoise.Play();
            if (raycast.collider.CompareTag("Inimigo"))
            {
                Inimigo inimigo = raycast.collider.GetComponent<Inimigo>();
                inimigo.CalculateDamageDir(raycast.point);
                if (raycast.collider == inimigo.headshotCollider && canHeadShot)
                {
                    if (inimigo.canbeStaggered)
                    {
                        Rigidbody rb = raycast.collider.GetComponent<Rigidbody>();
                        inimigo.Push();
                        rb.AddForce(direction * pushForce, ForceMode.Impulse);
                    }
                    inimigo.TakeDamage(damage * 2);
                    ultimate.AddUltPoints(damage * 2);

                }
                else if (!canHeadShot)
                {
                    if (inimigo.canbeStaggered)
                    {
                        Rigidbody rb = raycast.collider.GetComponent<Rigidbody>();
                        inimigo.Push();
                        rb.AddForce(direction * pushForce, ForceMode.Impulse);
                    }
                    ultimate.AddUltPoints(damage);
                    inimigo.TakeDamage(damage);
                }
                else
                {
                    ultimate.AddUltPoints(damage);
                    inimigo.TakeDamage(damage);
                }
            }
        }

        bulletsShot++;
        noiseCam.PlayNoise(new Vector3(recoil, recoil * 2, 0) * Time.deltaTime);

        if (bulletsShot < bulletsPerShot)
        {
            ContinueShootHitScan();
        }
        else
        {
            bulletsShot = 0;
            currentAmmo--;
            player?.playerUI?.UpdateAmmo(this);
            Invoke(nameof(ResetAttack), attackRate);
        }
        if(player!= null)
        {
            player.playerUI?.UpdateAmmo(this);
        }
    }

    void ContinueShootHitScan(){
        CmdShootHitScan();
    }

    [Command(requiresAuthority = false)]
    void CmdShootProjectile()
    {
        canShoot = false;
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        Vector3 direction = bulletPoint.transform.forward + new Vector3(spreadX, spreadY, 0);
        direction.Normalize();

        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            ProjectileBullet bala = Instantiate(bullet, bulletPoint.transform.position, Quaternion.Euler(bulletPoint.forward));
            bala.ult = ultimate;
            bala.target = raycast.point;
            bala.damage = damage;
            bala.pushForce = pushForce;
            NetworkServer.Spawn(bala.gameObject);
            bala.Move(this.gameObject);
            //shootNoise.Play();
        }

        bulletsShot++;
        noiseCam.PlayNoise(new Vector3(recoil / 2, recoil, 0) * Time.deltaTime);

        if (bulletsShot < bulletsPerShot)
        {
            ContinueShootProjectile();
        }
        else
        {
            bulletsShot = 0;
            currentAmmo--;
            player?.playerUI?.UpdateAmmo(this);
            Invoke(nameof(ResetAttack), attackRate);
        }
    }

    void ContinueShootProjectile()
    {
        CmdShootProjectile();
    }

    void ResetAttack()
    {
        canShoot = true;
    }
    public void Reload()
    {
        if(currentAmmo < maxAmmo)
        {
           // reloadNoise.Play();
            reloading = true;
            canShoot = false;
            Invoke(nameof(FinishReloading), reloadTime);
        }
    }

    void FinishReloading()
    {
        currentAmmo = maxAmmo;
        reloading = false;
        player.playerUI.UpdateAmmo(this);
        Invoke(nameof(ResetAttack), attackRate);
    }

    IEnumerator GenerateTrail(TrailRenderer t, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = t.transform.position;
        while(time < 1)
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
            canShoot = false;
            ProjectileBullet bala = Instantiate(bullet, bulletPoint.transform.position, Quaternion.Euler(bulletPoint.forward));
            bala.target = projectileTarget;
            bala.damage = damage;
            bala.pushForce = pushForce;
            NetworkServer.Spawn(bala.gameObject);
            bala.Move(enemyTarget);
          
            bulletsShot++;

            if (bulletsShot < bulletsPerShot)
            {
                ContinueShootEnemyProjectile(enemyTarget);
            }
            else
            {
                bulletsShot = 0;
                currentAmmo--;
                Invoke(nameof(ResetAttack), attackRate);
            }
        }
    }

    void ContinueShootEnemyProjectile(GameObject obj)
    {
        ShootEnemyProjectile();
    }
}
