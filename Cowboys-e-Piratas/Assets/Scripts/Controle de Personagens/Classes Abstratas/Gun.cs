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

    [Header("Se For projetil")]
    public bool projectile = false;
    public Vector3 projectileTarget;
    public ProjectileBullet bullet;

    [Header("Outros Componentes")]
    public NoiseCamera noiseCam;
    public Transform bulletPoint;
    RaycastHit raycast;
    public TrailRenderer trail;
    public Ultimate ultimate;
    public bool bufferedShot, bufferedReload;

    private void Awake()
    {
        reloading = false;
        canShoot = true;
        currentAmmo = maxAmmo;
    }

    public override void Action()
    {
        if(canShoot && !reloading && currentAmmo > 0)
        {
            bulletsShot = 0;
            if(!projectile)
            {
                CmdShootHitScan();
            }
            else
            {
                CmdShootProjectile();
            }
        }
    }

    //[Command(requiresAuthority = false)]
    void CmdShootHitScan()
    {
        canShoot = false;
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        Vector3 direction = bulletPoint.transform.forward + new Vector3(spreadX, spreadY, 0);
        direction.Normalize();

        if (Physics.Raycast(bulletPoint.transform.position, direction, out raycast, reach))
        {
            TrailRenderer bulletTrail = Instantiate(trail, bulletPoint.transform.position, Quaternion.Euler(bulletPoint.forward));
            StartCoroutine(GenerateTrail(bulletTrail, raycast));
           // NetworkServer.Spawn(bulletTrail.gameObject);
            if(raycast.collider.CompareTag("Inimigo"))
            {
                Inimigo inimigo = raycast.collider.GetComponent<Inimigo>();
                if(raycast.collider == inimigo.headshotCollider && canHeadShot)
                {
                    if(inimigo.staggerable)
                    {
                        Rigidbody rb = raycast.collider.GetComponent<Rigidbody>();
                        inimigo.Push();
                        rb.AddForce(direction * pushForce, ForceMode.Impulse);
                    }
                    inimigo.TomarDano(damage * 2);
                    ultimate.ganharUlt(damage * 2);

                }
                else if(!canHeadShot)
                {
                    if(inimigo.staggerable)
                    {
                        Rigidbody rb = raycast.collider.GetComponent<Rigidbody>();
                        inimigo.Push();
                        rb.AddForce(direction * pushForce, ForceMode.Impulse);
                    }
                    ultimate.ganharUlt(damage);
                    inimigo.TomarDano(damage);
                }
                else
                {
                    ultimate.ganharUlt(damage);
                    inimigo.TomarDano(damage);
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
            Invoke("ResetAttack", attackRate);
        }
    }

    void ContinueShootHitScan(){
        CmdShootHitScan();
    }

    //[Command(requiresAuthority = false)]
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
            //NetworkServer.Spawn(bala.gameObject);
            bala.Move(this.gameObject);
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
            Invoke("ResetAttack", attackRate);
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
            reloading = true;
            canShoot = false;
            Invoke("FinishReloading", reloadTime);
        }
    }

    void FinishReloading()
    {
        currentAmmo = maxAmmo;
        reloading = false;
        UIManagerCowboy.instance.AttAmmo(this);
        Invoke("ResetAttack", attackRate);
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

    //[Command(requiresAuthority = false)]
    public void CmdShootEnemyProjectile(GameObject obj)
    {
        if (canShoot)
        {
            canShoot = false;
            ProjectileBullet bala = Instantiate(bullet, bulletPoint.transform.position, Quaternion.Euler(bulletPoint.forward));
            bala.target = projectileTarget;
            bala.damage = damage;
            bala.pushForce = pushForce;
            //NetworkServer.Spawn(bala.gameObject);
            bala.Move(obj);
          
            bulletsShot++;

            if (bulletsShot < bulletsPerShot)
            {
                ContinueShootEnemyProjectile(obj);
            }
            else
            {
                bulletsShot = 0;
                currentAmmo--;
                Invoke("ResetAttack", attackRate);
            }
        }
    }

    void ContinueShootEnemyProjectile(GameObject obj)
    {
        CmdShootEnemyProjectile(obj);
    }
}
