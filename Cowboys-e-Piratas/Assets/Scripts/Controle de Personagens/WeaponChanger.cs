using System.Collections;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] Personagem personagem;

    public void DisableWeapon(int index)
    {
        if (index >= 0 && index < personagem.weapons.Count)
        {
            DoStuff(index, false);
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    public void EnableWeapon(int index)
    {
        if (index >= 0 && index < personagem.weapons.Count)
        {
            DoStuff(index, true);
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }

    void DoStuff(int index, bool state)
    {
        personagem.CmdSetGunState(index, state);
    }
}
