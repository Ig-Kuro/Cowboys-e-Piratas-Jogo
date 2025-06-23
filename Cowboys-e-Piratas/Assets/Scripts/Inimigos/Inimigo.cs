using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public abstract class Inimigo : NetworkBehaviour
{
    public bool staggerable = true, stunned;
    public int health;
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
    private bool dead = false;
    public DamageInfo damage;
    public Animator anim;
    public bool canAttack;
    public bool canbeStaggered;

    public GameObject bracoDireito, bracoEsquerdo;

    public GameObject looseArmFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        damage = new DamageInfo();
        anim.SetBool("Walking", true);
        canAttack = true;
        canbeStaggered = true;
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
    public virtual void TakeDamage(int value)
    {
        if (dead) return;
        health -= value;
        if (canbeStaggered)
        {
            anim.SetBool("Dano", true);
            DecideDamageAnimation();
            canbeStaggered = false;
            Push();
            Invoke("CanBeStaggeredAgain", 2f);
        }
        if (health < 0)
        {
            dead = true;
            if (WaveManager.instance != null)
            {
                WaveManager.instance.OnEnemyKilled();
            }
            NetworkServer.Destroy(gameObject);
        }
    }

    [Server]
    void DecideDamageAnimation()
    {
        if (damage.damageDirection == DamageInfo.DamageDirection.Right)
        {
            anim.SetTrigger("DanoDir");
        }
        else
        {
            anim.SetTrigger("DanoEsq");
        }
        anim.SetBool("Dano", false);
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
            stunned = false;
            anim.SetBool("Walking", false);
        }
    }

    [Server]
    public void KnockUp(float force, int damage)
    {
        if (!recovering)
        {
            Stun();
            rb.AddForce(rb.transform.up * force, ForceMode.Impulse);
            TakeDamage(damage);
        }
    }

    [Server]
    public void Stun()
    {
        recovering = true;
        stunned = true;
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
        canAttack = true;
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


    public void CalculateDamageDir(Vector3 point)
    {
        if(point.x - rb.centerOfMass.x >= 0.5)
        {
            damage.damageDirection = DamageInfo.DamageDirection.Right;
        }
        else
        {
            damage.damageDirection = DamageInfo.DamageDirection.Left;
        }

    }

}
