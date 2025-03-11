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
    public static List <InimigoTeste> inims = new List<InimigoTeste>();
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Invoke("Throw", activationTime);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    void Throw()
    {
        rb.useGravity = true;
        rb.AddForce(transform.parent.forward * throwSpeed, ForceMode.Impulse);
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetEnemies();
        rb.constraints = RigidbodyConstraints.FreezeAll;
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
