using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class EnemyBehaviour : NetworkBehaviour
{
    public enum EnemyState { Idle, Chasing, Attacking, Stunned }
    public EnemyState currentState;

    public NavMeshAgent agent;
    public Transform target;
    private Inimigo inimigo;

    [Header("IA")]
    public float sightRange = 20f;
    public float attackRange = 2.5f;
    public float timeBetweenAttacks = 1.5f;
    public bool isRanged = false;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        inimigo = GetComponent<Inimigo>();
    }

    private void Start()
    {
        if (!isServer) return;

        TargetManager.instance.RegisterEnemy(this);
        UpdateTarget();
    }

    private void OnDestroy()
    {
        if (isServer)
            TargetManager.instance.UnregisterEnemy(this);
    }

    private void Update()
    {
        if (!isServer || inimigo.dead) return;

        if (inimigo.recovering)
        {
            currentState = EnemyState.Stunned;
            return;
        }

        if (target == null)
        {
            UpdateTarget();
            currentState = EnemyState.Idle;
            agent.SetDestination(transform.position);
            return;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= attackRange)
        {
            currentState = EnemyState.Attacking;
            inimigo.PerformAttack();
        }
        else if (dist <= sightRange)
        {
            currentState = EnemyState.Chasing;
            Chase();
        }
        else
        {
            currentState = EnemyState.Idle;
            agent.SetDestination(transform.position);
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        if (inimigo.anim != null)
        {
            inimigo.anim.SetBool("Walking", currentState == EnemyState.Chasing);
        }
    }

    void Chase()
    {
        if (target != null)
            agent.SetDestination(target.position);
    }

    public void UpdateTarget()
    {
        target = TargetManager.instance.GetClosestTarget(transform.position);
    }
}
