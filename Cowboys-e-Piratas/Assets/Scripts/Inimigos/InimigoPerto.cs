using System.Collections.Generic;
using UnityEngine;

public class InimigoPerto : Inimigo
{
    public GameObject[] players;
    public Transform target;
    public MeleeWeapon weapon;
    public bool moveWhileAttacking;

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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!moveWhileAttacking)
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
