using Unity.VisualScripting;
using UnityEngine;

public class InimigoRapido : Inimigo
{
    public GameObject[] players;
    public Transform target;
    public MeleeWeapon weapon;
    public bool moveWhileAttacking;
    public bool canDodge;
    Personagem player;
    public float dodgeSpeed, dodgeTimer;
    bool visivel;
    RaycastHit ray;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int alvo = Random.Range(0, players.Length);
        target = players[alvo].transform;
        player = players[alvo].GetComponent<Personagem>();

    }
    void FixedUpdate()
    {
        if (agent.enabled)
        {
            agent.destination = target.position;
            Vector3 direction = attackPoint.transform.forward;
            if (Physics.Raycast(attackPoint.position, direction, out ray, attackRange))
            {
                if (ray.collider.CompareTag("Player"))
                {
                    if (!moveWhileAttacking)
                    {
                        agent.enabled = false;
                        rb.isKinematic = false;
                        recovering = true;
                        Invoke("Recovery", weapon.delay + weapon.attackRate);
                    }
                    weapon.Action();
                }
            }
        }
    }

    private void Update()
    {
        if(player.input.AttackInput())
        {
            if(canDodge)
            {
                Dodge();
            }
        }
    }

    void Dodge()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        int lado = Random.Range(0, 2);
        if(lado == 0)
        {
            rb.AddForce(Vector3.right * dodgeSpeed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(-Vector3.right * dodgeSpeed, ForceMode.Impulse);
        }
        canDodge = false;
        weapon.canAttack = false;
        Invoke("RecoverDodge", dodgeTimer);

    }

    void RecoverDodge()
    {
        canDodge = true;
        agent.enabled = true;
        rb.isKinematic = true;
        weapon.canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, new Vector3(attackPoint.position.x, attackPoint.position.y, attackPoint.position.z - attackRange));
    }
}
