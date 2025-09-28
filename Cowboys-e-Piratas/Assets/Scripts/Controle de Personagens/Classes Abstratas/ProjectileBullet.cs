using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public Vector3 target;
    public float movementSpeed;
    public Ultimate ult;
    public int damage;
    public float pushForce;
    public bool bounce;
    public bool canHeadshot = true;
    public bool destructable = true;
    public float lifeTime;
    Rigidbody rb;
    public enum TypeOfBullet { Player, Enemy }
    public TypeOfBullet type;

    [Header("Explosivo")]
    public bool explosive = false;
    public float areaOfEffect;
    public LayerMask layer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(!destructable)
        {
            Destroy(this.gameObject, lifeTime);
        }
    }
    public void Move(GameObject obj)
    {
        transform.rotation = obj.transform.rotation;
        rb.AddForce(transform.forward * movementSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Inimigo") && type == TypeOfBullet.Player)
        {
            Inimigo inimigo = col.gameObject.GetComponent<Inimigo>();
            if (col.collider == inimigo.headshotCollider && canHeadshot)
            {
                if (inimigo.canbeStaggered)
                {
                    Rigidbody rbi = col.gameObject.GetComponent<Rigidbody>();
                    inimigo.Push();
                    rbi.AddForce(rb.transform.forward * pushForce, ForceMode.Impulse);
                }
                inimigo.TakeDamage(damage * 2);
                //ult.AddUltPoints(damage * 2);
            }
            else
            {
                inimigo.TakeDamage(damage);
            }

            if(explosive)
            {
                Explode(transform.position);
            }


            if (!bounce && destructable)
            {
                Destroy(this.gameObject);
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }

        else if (col.gameObject.CompareTag("Player") && type == TypeOfBullet.Enemy)
        {
            Personagem player = col.gameObject.GetComponent<Personagem>();
            player.TakeDamage(damage);
        }

        if (!bounce && destructable)
        {
            Destroy(this.gameObject);
        }
        else if(bounce)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }


    void Explode(Vector3 position)
    {
        Collider[] coliders = Physics.OverlapSphere(position, areaOfEffect, layer);

        foreach (Collider col in coliders)
        {
            if (col.TryGetComponent(out Inimigo enemy))
            {
                //GameObject blood = Instantiate(bloodFX, col.transform.position, enemy.transform.rotation);
                enemy.TakeDamage(damage);
            }
        }
        if(destructable)
            Destroy(this.gameObject);
    }
}
