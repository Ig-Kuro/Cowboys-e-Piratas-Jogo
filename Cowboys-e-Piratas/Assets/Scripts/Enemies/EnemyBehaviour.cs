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
    public float attackRange = 2.5f;
    public float timeBetweenAttacks = 1.5f;
    public bool isRanged = false;
    public GameObject bulletPrefab;
    public Transform shootPoint;

    private void Awake()
    {
        inimigo = GetComponent<Inimigo>();
    }

    private void Start()
    {
        if (!isServer) return;

        TargetManager.instance.RegisterEnemy(this);
        TargetManager.instance.GetRandomTarget();
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
        else
        {
            currentState = EnemyState.Chasing;
            Chase();
        }
        FaceTarget();
        UpdateAnimations();
    }

    void FaceTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void UpdateAnimations()
    {
        if (inimigo.anim != null)
        {
            inimigo.anim.SetBool("Walk", currentState == EnemyState.Chasing);
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
