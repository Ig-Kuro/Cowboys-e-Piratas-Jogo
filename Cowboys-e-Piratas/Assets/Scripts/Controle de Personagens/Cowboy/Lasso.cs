using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Lasso : MonoBehaviour
{
    public float rotationSpeed;
    public float checkRadius;
    public float throwSpeed;
    public float pullForce;
    public float activationTime;
    public float maxEnemyCount;
    bool thrown;
    Collider col;
    public static List <InimigoTeste> inims = new List<InimigoTeste>();
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        thrown = false;
        col = GetComponent<Collider>(); 
        Invoke("Throw", activationTime);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        if(transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }
    void Throw()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;
        col.enabled = true;
        rb.AddForce(transform.parent.forward * throwSpeed, ForceMode.Impulse);
        transform.SetParent(null);
        thrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(thrown)
        {
            GetEnemies();
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void GetEnemies()
    {
        Collider[] colider = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider col in colider)
        {
            if(col.gameObject.GetComponent<InimigoTeste>() != null)
            {
                if(inims.Count < maxEnemyCount)
                {
                    inims.Add(col.gameObject.GetComponent<InimigoTeste>()) ;
                }
            }
        }
        PullEnemies();
    }

    void PullEnemies()
    {
        if(inims.Count > 0)
        {
            foreach (InimigoTeste it in  inims)
            {
                it.rb.AddForce((transform.position - it.transform.position).normalized * pullForce, ForceMode.Impulse);
                it.Stun();
            }
        }
    }
}
