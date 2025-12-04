using System.Collections;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] Personagem personagem;

    public void DisableWeapon(int index, float delay = 0f)
    {
        if (index >= 0 && index < personagem.weapons.Count)
        {
            StartCoroutine(DoStuff(index, false, delay));
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    public void EnableWeapon(int index, float delay = 0f)
    {
        if (index >= 0 && index < personagem.weapons.Count)
        {
            StartCoroutine(DoStuff(index, true, delay));
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    IEnumerator DoStuff(int index, bool state, float delay)
    {
        yield return new WaitForSeconds(delay);
        personagem.CmdSetGunState(index, state);
    }
}
