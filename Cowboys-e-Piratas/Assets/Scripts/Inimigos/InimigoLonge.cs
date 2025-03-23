using UnityEngine;

public class InimigoLonge : Inimigo
{
    public GameObject[] players;
    public Transform target;
    public bool moveWhileAttacking;
    public Gun weapon;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int alvo = Random.Range(0, players.Length);
        target = players[alvo].transform;
    }

    void FixedUpdate()
    {
        if (agent.enabled)
        {
            agent.destination = target.position;
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
                Invoke("Recovery", weapon.attackRate);
            }
            weapon.projectileTarget = target.gameObject.GetComponent<Rigidbody>().centerOfMass;
            weapon.ShootEnemyProjectile();
        }
    }
}
