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
    public Collider headshotCollider;
    public Transform attackPoint;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void TomarDano(int valor)
    {
        vida -= valor;
        if(vida < 0)
        {
            if(SpawnManager.instance!=null)
            {
                SpawnManager.instance.inimigosSpawnado.Remove(this);
            }
            else if(WaveManager.instance!=null)
            {
                WaveManager.instance.currentenemies--;
            }
            Destroy(this.gameObject);
        }
    }

    public void Push()
    {
        if (!recovering)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            Invoke("Recovery", 0.5f);
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
        recovering = true;
        stunado = true;
        agent.enabled = false;
        rb.isKinematic = false;
        Invoke("Recovery", stunTime);
    }
}
