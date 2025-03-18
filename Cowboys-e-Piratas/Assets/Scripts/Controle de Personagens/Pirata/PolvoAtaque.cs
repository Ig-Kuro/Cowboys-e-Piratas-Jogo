using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PolvoAtaque : MonoBehaviour
{
    public static List<InimigoTeste> inims = new List<InimigoTeste>();
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
        if(other.GetComponent<InimigoTeste>() != null)
        {
            inims.Add(other.GetComponent<InimigoTeste>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InimigoTeste>() != null)
        {
            inims.Remove(other.GetComponent<InimigoTeste>());
        }
    }
    void CauseDamage()
    {
        if (inims.Count > 0)
        {
            foreach (InimigoTeste it in inims)
            {
                it.rb.AddForce(it.rb.transform.up * throwStrength, ForceMode.Impulse);
                it.Stun();
                it.TomarDano(damage);
            }
        }
        timer = 0;
    }
}
