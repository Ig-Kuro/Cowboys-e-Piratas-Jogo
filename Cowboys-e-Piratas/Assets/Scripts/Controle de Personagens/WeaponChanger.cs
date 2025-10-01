using System.Collections;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public GameObject[] weapons;

    public void DisableWeapon(int index, float delay)
    {
        if (index >= 0 && index < weapons.Length)
        {
            StartCoroutine(DoStuff(index, delay, false));
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    public void EnableWeapon(int index, float delay)
    {
        if (index >= 0 && index < weapons.Length)
        {
            StartCoroutine(DoStuff(index, delay, true));
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    IEnumerator DoStuff(int index, float delay, bool state)
    {
        yield return new WaitForSeconds(delay);
        weapons[index].SetActive(state);
    }
}
