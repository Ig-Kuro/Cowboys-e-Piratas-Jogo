using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;

    private HashSet<EnemyBehaviour> enemies = new();

    [SerializeField] private List<Personagem> players = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void RegisterEnemy(EnemyBehaviour enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyBehaviour enemy)
    {
        enemies.Remove(enemy);
    }

    public void RegisterPlayer(Personagem player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void UnregisterPlayer(Personagem player)
    {
        players.Remove(player);
    }

    public Transform GetClosestTarget()
    {
        Debug.Log("Getting closest target using PriorityQueue and List fallback.");


        PriorityQueue<Personagem> pq = new PriorityQueue<Personagem>();

        foreach (var p in players)
        {
            if (p == null || p.dead) continue;

            float dist = Vector3.Distance(transform.position, p.transform.position);
            pq.Enqueue(p, dist); 
        }

        Personagem closestPQ = null;
        if (pq.Count > 0)
            closestPQ = pq.Dequeue();

        if (players == null || players.Count == 0)
            return null;

        List<float> distances = new List<float>();
        foreach (var p in players)
        {
            distances.Add(Vector3.Distance(transform.position, p.transform.position));
        }

        int closestIndex = 0;
        if (distances.Count > 0)
        {
            float min = distances[0];
            for (int i = 1; i < distances.Count; i++)
            {
                if (distances[i] < min)
                {
                    min = distances[i];
                    closestIndex = i;
                }
            }
        }

        return players[closestIndex].transform;
    }


    public Transform GetClosestTarget(Vector3 fromPos)
    {
        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var p in players)
        {
            if (p == null || p.dead) continue;
            float dist = Vector3.Distance(fromPos, p.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = p.transform;
            }
        }

        return closest;
    }

    public void NotifyPlayerDeath(Personagem deadPlayer)
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            if (enemy.target == deadPlayer.transform)
            {
                enemy.UpdateTarget();
            }
        }
    }
}
