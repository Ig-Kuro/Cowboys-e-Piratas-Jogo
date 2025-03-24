using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class Inimigo : MonoBehaviour
{
    public bool staggerable = true, stunado;
    public int vida;
    public bool recovering;
    public float stunTime;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public float attackRange;
    public Transform attackPoint;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void TomarDano(int valor)
    {
        vida -= valor;
        Debug.Log("ui");
    }

    public void Push()
    {
        if (!recovering)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            Invoke("Recovery", 0.2f);
            recovering = true;
            stunado = false;
        }
    }

    public void Recovery()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        recovering = false;
    }

    public void Stun()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        Invoke("Recovery", stunTime);
        recovering = true;
        stunado = true;
    }
}
