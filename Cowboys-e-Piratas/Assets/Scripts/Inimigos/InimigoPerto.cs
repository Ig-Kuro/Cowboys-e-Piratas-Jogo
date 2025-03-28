using System.Collections.Generic;
using UnityEngine;

public class InimigoPerto : Inimigo
{
    public GameObject[] players;
    public Transform target;
    public MeleeWeapon weapon;
    public bool moveWhileAttacking;
    bool visivel;
    RaycastHit ray;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int alvo = Random.Range(0, players.Length);
        target = players[alvo].transform;
    }
    void FixedUpdate()
    {
        if(agent.enabled)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackPoint.position, new Vector3(attackPoint.position.x, attackPoint.position.y, attackPoint.position.z - attackRange));
    }
}
