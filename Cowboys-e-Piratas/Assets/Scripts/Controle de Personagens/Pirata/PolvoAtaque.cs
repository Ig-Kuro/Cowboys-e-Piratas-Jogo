using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class PolvoAtaque : NetworkBehaviour
{
    private List<Inimigo> inims = new();

    public float timeBetweenAttacks;
    private float timer;
    public int damage;
    public float throwStrength;
    public Animator[] animators;

    private void Awake()
    {
        Invoke(nameof(EndSkill), 15f);
        foreach (Animator anim in animators)
        {
            anim.SetBool("Ativo", true);
        }
    }

    private void FixedUpdate()
    {
        if (!isServer) return; // apenas o servidor processa

        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks)
        {
            CauseDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        Inimigo inim = other.GetComponent<Inimigo>();
        if (inim != null && !inims.Contains(inim))
        {
            inims.Add(inim);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isServer) return;

        if (other.TryGetComponent<Inimigo>(out var inim))
        {
            inims.Remove(inim);
        }
    }

    [Server]
    void CauseDamage()
    {
        foreach (Inimigo it in inims)
        {
            if (it != null)
            {
                it.KnockUp(throwStrength, damage);
            }
        }

        timer = 0;
    }

    void EndSkill()
    {
        foreach(Animator anim in animators)
        {
            anim.SetBool("Ativo", false);
        }
        Destroy(this.gameObject, 2f);
    }

}
