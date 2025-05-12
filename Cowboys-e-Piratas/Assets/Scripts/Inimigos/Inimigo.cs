using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public abstract class Inimigo : NetworkBehaviour
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
    public GameObject[] players;
    public Transform target;
    public bool moveWhileAttacking;
    public RaycastHit ray;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    [ServerCallback]
    public virtual void Start()
    {
        List<Transform> possibleTargets = new();

        // Busca de players no servidor
        foreach (NetworkIdentity identity in NetworkServer.spawned.Values)
        {
            if (identity.TryGetComponent(out PlayerObjectController character))
            {
                possibleTargets.Add(character.transform);
            }
        }

        if (possibleTargets.Count > 0)
        {
            int targetIndex = Random.Range(0, possibleTargets.Count);
            target = possibleTargets[targetIndex];
        }
        else
        {
            Debug.LogWarning("Nenhum player encontrado para o inimigo mirar.");
        }

        Recovery();
    }


    [Server]
    public void TomarDano(int valor)
    {
        vida -= valor;
        if(vida < 0)
        {

            if(WaveManager.instance!=null)
            {
                WaveManager.instance.OnEnemyKilled();
                WaveManager.instance.CheckIfWaveEnded();
            }
            NetworkServer.Destroy(gameObject);
        }
    }

    [Server]
    public void Push()
    {
        if (!recovering)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            Invoke(nameof(Recovery), 0.5f);
            recovering = true;
            stunado = false;
        }
    }

    [Server]
    public void KnockUp(float force, int damage)
    {
        if (!recovering)
        {
            Stun();
            rb.AddForce(rb.transform.up * force, ForceMode.Impulse);
            TomarDano(damage);
        }
    }

    [Server]
    public void Stun()
    {
        recovering = true;
        stunado = true;
        agent.enabled = false;
        rb.isKinematic = false;
        Invoke(nameof(Recovery), stunTime);
    }

    [Server]
    public void Recovery()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        recovering = false;
    }

    // ClientRpc para aplicar efeitos visuais do recovery nos clientes
    [ClientRpc]
    void RpcRecover()
    {
        if (!isServer)
        {
            agent.enabled = true;
            rb.isKinematic = true;
        }
    }


}
