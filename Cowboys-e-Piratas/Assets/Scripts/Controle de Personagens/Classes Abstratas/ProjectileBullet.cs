using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public Vector3 target;
    public float movementSpeed;
    public Ultimate ult;
    public int damage;
    public float pushForce;
    public bool bounce;
    Rigidbody rb;   
    public enum TypeOfBullet { Player, Enemy}
    public TypeOfBullet type;
    private void Awake()
    {
        Destroy(this.gameObject, 7f);
        rb = GetComponent<Rigidbody>();
    }
    public void Move(GameObject obj)
    {
        transform.rotation = obj.transform.rotation;
        rb.AddForce(transform.forward * movementSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Inimigo") && type == TypeOfBullet.Player)
        {
            Inimigo inimigo = col.gameObject.GetComponent<Inimigo>();
            if (col.collider == inimigo.headshotCollider)
            {
                if(inimigo.canbeStaggered)
                {
                    Rigidbody rbi = col.gameObject.GetComponent<Rigidbody>();
                    inimigo.Push();
                    rbi.AddForce( rb.transform.forward* pushForce, ForceMode.Impulse);
                }
                inimigo.TakeDamage(damage * 2);
                //ult.AddUltPoints(damage * 2);
            }
            else
            {
                inimigo.TakeDamage(damage);
            }
            
            if (!bounce)
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
            Rigidbody rbp = col.gameObject.GetComponent<Rigidbody>();
            rbp.AddForce(rb.transform.forward * pushForce, ForceMode.Impulse);
            player.TakeDamage(damage);
        }

        if (!bounce)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
