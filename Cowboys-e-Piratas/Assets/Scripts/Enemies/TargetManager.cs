using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;

    private List<EnemyBehaviour> enemies = new();
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
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyBehaviour enemy)
    {
        enemies.Remove(enemy);
    }

    public void RegisterPlayer(Personagem player)
    {
        Debug.Log("Player registered: " + player.name);
        if (!players.Contains(player))
            players.Add(player);
    }

    public void UnregisterPlayer(Personagem player)
    {
        players.Remove(player);
    }

    public Transform GetRandomTarget()
    {
        Debug.Log("Getting random target from " + players.Count + " players.");
        int randomIndex = Random.Range(0, players.Count);
        return players[randomIndex].transform;
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
