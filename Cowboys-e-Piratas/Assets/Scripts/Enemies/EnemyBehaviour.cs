
using UnityEngine;
using UnityEngine.AI;

//Usa esse vídeo de referência: https://www.youtube.com/watch?v=UjkSFoLxesw&t=216s
//Mantenha em mente q nosso jogo vai ter inimigo ranged e melee, ent o código tem q poder ser utilizado por ambos

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Variaveis de Patrulha
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Variaveis de Ataque
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Ranges
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player= GameObject.FindWithTag("Player").transform;
        agent= GetComponent<NavMeshAgent>(); 
    }
    private void Update()
    {
        playerInSightRange= Physics.CheckSphere(transform.position,sightRange,whatIsPlayer);
        playerInAttackRange= Physics.CheckSphere(transform.position,attackRange,whatIsPlayer);
    
        if(!playerInSightRange&&!playerInAttackRange)Patroll();
        if(playerInSightRange&&!playerInAttackRange)Chase();
        if(playerInSightRange&&playerInAttackRange)AttackPlayer();
    }
    private void Patroll()
    {
        if(!walkPointSet) GetWalkPoint();

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distancetoWalkPoint = transform.position - walkPoint;

            if(distancetoWalkPoint.magnitude<1f)
            {
                walkPointSet= false;
            }
        }
    }
    private void GetWalkPoint()
    {
        float randomZ= Random.Range(-walkPointRange, walkPointRange);
        float randomX= Random.Range(-walkPointRange, walkPointRange);
        walkPoint= new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint,-transform.up,2f,whatIsGround))
        {
            walkPointSet =true;
        }

    }
    private void Chase()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        if(!alreadyAttacked)
        {
            alreadyAttacked= true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked=false;
    }
}
