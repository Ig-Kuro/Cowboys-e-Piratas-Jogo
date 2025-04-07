using Unity.VisualScripting;
using UnityEngine;

public class InimigoLonge : Inimigo
{
    public GameObject[] players;
    public Transform target;
    public bool moveWhileAttacking;
    bool visivel;
    RaycastHit ray;
    public Gun weapon;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int alvo = Random.Range(0, players.Length);
        target = players[alvo].transform;
        Recovery();
    }

    void FixedUpdate()
    {
        if (agent.enabled)
        {
            agent.destination = target.position;
            if (Physics.Raycast(attackPoint.position, attackPoint.transform.forward, out ray, attackRange))
            {
                if (ray.collider.CompareTag("Player"))
                {
                    Debug.Log("Cu");
                    if (!moveWhileAttacking)
                    {
                        agent.enabled = false;
                        rb.isKinematic = false;
                        recovering = true;
                        Invoke("Recovery", weapon.attackRate);
                    }
                    weapon.CmdShootEnemyProjectile(attackPoint.gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, ray.point);
    }
}
