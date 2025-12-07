using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public abstract class Inimigo : NetworkBehaviour
{
    public int health;
    public float stunTime;
    public bool recovering, dead, canAttack, canbeStaggered = true;
    public RagdollController ragdoll;
    public AudioSource ataqueAudio;


    public Rigidbody rb;
    public NavMeshAgent agent;
    public Animator anim;
    public Collider headshotCollider;

    public Transform attackPoint;
    public float attackRange = 1f;
    public bool moveWhileAttacking = true;

    public DamageInfo damage;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;
    Coroutine flashCoroutine;

    public GameObject looseArmFX;

    void Awake()
    {
        damage = ScriptableObject.CreateInstance<DamageInfo>();
        canAttack = true;
        ragdoll = GetComponent<RagdollController>();
        if (meshRenderers.Length < 1)
            meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var r in meshRenderers)
            r.material = new Material(r.material); // clone para não afetar outros
    }

    public abstract void PerformAttack();

    [Server]
    public virtual void TakeDamage(int value)
    {
        if (dead) return;

        health -= value;
        RpcFlashRed();

        if (canbeStaggered)
        {
            anim.SetTrigger(GetDamageDirectionTrigger());
            Stagger();
        }

        if (health <= 0)
        {
            dead = true;
            Die();
        }
    }

    [Server]
    public virtual void Die()
    {
        if (WaveManager.instance != null)
            WaveManager.instance.OnEnemyKilled();

        agent.enabled = false;
        anim.enabled = false;
    }

    string GetDamageDirectionTrigger()
    {
        return damage.damageDirection == DamageInfo.DamageDirection.Right ? "DanoDir" : "DanoEsq";
    }

    [Server]
    public void Stagger()
    {
        recovering = true;
        rb.isKinematic = false;
        agent.enabled = false;
        anim.SetBool("Walking", false);
        Invoke(nameof(Recover), 0.5f);
    }

    [Server]
    public void KnockUp(float force, int damage)
    {
        if (!recovering)
        {
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            Stun();
            TakeDamage(damage);
        }
    }

    [Server]
    public void Stun()
    {
        recovering = true;
        agent.enabled = false;
        rb.isKinematic = false;
        Invoke(nameof(Recover), stunTime);
    }

    [Server]
    public void Push()
    {
        if (!recovering)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            Invoke(nameof(Recover), 0.5f);
            recovering = true;
            anim.SetBool("Walking", false);
        }
    }

    [Server]
    public void Recover()
    {
        recovering = false;
        canAttack = true;
        rb.isKinematic = true;
        agent.enabled = true;
        RpcRecover();
    }

    [ClientRpc]
    void RpcRecover()
    {
        if (!isServer)
        {
            rb.isKinematic = true;
            agent.enabled = true;
        }
    }

    [ClientRpc]
    void RpcFlashRed()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        List<Color[]> originalColors = new();
        foreach (var r in meshRenderers)
        {
            Color[] colors = new Color[r.materials.Length];
            for (int i = 0; i < r.materials.Length; i++)
            {
                colors[i] = r.materials[i].color;
                r.materials[i].color = Color.red;
            }
            originalColors.Add(colors);
        }

        yield return new WaitForSeconds(0.1f);

        for (int r = 0; r < meshRenderers.Length; r++)
        {
            for (int i = 0; i < meshRenderers[r].materials.Length; i++)
                meshRenderers[r].materials[i].color = originalColors[r][i];
        }
    }

    public void CalculateDamageDir(Vector3 point)
    {
        if (point.x - rb.centerOfMass.x >= 0.5f)
            damage.damageDirection = DamageInfo.DamageDirection.Right;
        else
            damage.damageDirection = DamageInfo.DamageDirection.Left;
    }
}