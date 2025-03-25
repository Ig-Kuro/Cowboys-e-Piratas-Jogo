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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
}
