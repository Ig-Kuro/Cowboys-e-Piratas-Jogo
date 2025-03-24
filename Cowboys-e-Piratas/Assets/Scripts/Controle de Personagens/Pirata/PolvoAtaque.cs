using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PolvoAtaque : MonoBehaviour
{
    public static List<Inimigo> inims = new List<Inimigo>();
    public float timeBetweenAttacks;
    float timer;
    public int damage;
    public float throwStrength;

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer >= timeBetweenAttacks)
        {
            CauseDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Inimigo>() != null)
        {
            inims.Add(other.GetComponent<Inimigo>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Inimigo>() != null)
        {
            inims.Remove(other.GetComponent<Inimigo>());
        }
    }
    void CauseDamage()
    {
        if (inims.Count > 0)
        {
            foreach (Inimigo it in inims)
            {
                it.rb.AddForce(it.rb.transform.up * throwStrength, ForceMode.Impulse);
                it.Stun();
                it.TomarDano(damage);
            }
        }
        timer = 0;
    }
}
