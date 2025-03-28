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

    private void Awake()
    {
        if(bounce)
        {
            Destroy(this.gameObject, 5f);
        }
        rb = GetComponent<Rigidbody>();
    }
    public void Move(GameObject obj)
    {
        rb.AddForce(obj.transform.forward * movementSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Inimigo"))
        {
            Inimigo inimigo = col.gameObject.GetComponent<Inimigo>();
            if (inimigo.staggerable)
            {
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                inimigo.Push();
                rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
            }
            inimigo.TomarDano(damage);
            //ult.ganharUlt(damage);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            Personagem player = col.gameObject.GetComponent<Personagem>();
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(-transform.forward * pushForce, ForceMode.Impulse);
            player.TomarDano(damage);
        }
        if(!bounce)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
