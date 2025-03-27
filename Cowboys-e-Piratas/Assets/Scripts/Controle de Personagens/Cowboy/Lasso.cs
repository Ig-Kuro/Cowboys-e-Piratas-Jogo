using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Mirror;
using StinkySteak.NetcodeBenchmark;

public class Lasso : NetworkBehaviour
{
    public float rotationSpeed;
    public float checkRadius;
    public float throwSpeed;
    public float pullForce;
    public float activationTime;
    public float maxEnemyCount;
    bool thrown;
    Collider col;
    public static List <Inimigo> inims = new List<Inimigo>();
    Rigidbody rb;
    Transform parent;
    Vector3 direction;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        thrown = false;
        col = GetComponent<Collider>();
        parent = transform.parent;
        Invoke(nameof(Throw), activationTime);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        if(transform.parent != null)
        {
            transform.position = transform.parent.position;
        }
    }

    [Command(requiresAuthority = false)]
    void Throw()
    {
        direction = parent.forward;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;
        
        col.enabled = true;
        rb.AddForce(direction * throwSpeed, ForceMode.Impulse);
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
            if(col.gameObject.GetComponent<Inimigo>() != null)
            {
                if(inims.Count < maxEnemyCount)
                {
                    inims.Add(col.gameObject.GetComponent<Inimigo>()) ;
                }
            }
        }
        PullEnemies();
    }

    void PullEnemies()
    {
        if(inims.Count > 0)
        {
            foreach (Inimigo it in  inims)
            {
                it.rb.isKinematic = false;
                it.agent.enabled = false;
                it.rb.AddForce((transform.position - it.transform.position).normalized * pullForce, ForceMode.Impulse);
                it.Stun();
            }
        }
    }
}
