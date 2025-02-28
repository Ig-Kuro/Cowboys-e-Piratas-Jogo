using UnityEngine;
using UnityEngine.AI;

//Usa esse vídeo de referência: https://www.youtube.com/watch?v=UjkSFoLxesw&t=216s

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;
    [SerializeField]
    private GameObject currentTarget;
    [SerializeField]
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        players=GameObject.FindGameObjectsWithTag("Player");
        //agent.destination= new Vector3(100,100,100);
        for(int i = 0; i < players.Length; i++)
        {
            if(agent.destination==null)
            {
                agent.destination=players[i].transform.position;
            }
            else
            {
                if(Vector3.Distance (transform.position, players[i].transform.position)>Vector3.Distance (transform.position, agent.destination))
                {
                    agent.destination=players[i].transform.position;
                    currentTarget=players[i];
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, agent.destination) <= 1)
        {
            Invoke("Ataque",1);
        }
    }
    void Ataque()
    {
        currentTarget.GetComponent<Movimentacao>().FuiAtacado();
    }
    //Vector3.Distance (this.transform.position, players[i].transform.position)>1
}
