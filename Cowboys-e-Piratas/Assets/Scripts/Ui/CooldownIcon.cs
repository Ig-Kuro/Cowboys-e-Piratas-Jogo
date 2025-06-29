using UnityEngine;
using UnityEngine.UI;

public class CooldownIcon : MonoBehaviour
{
    public float cooldownTime;
    public bool inCooldown;

    public Image cooldownImage;
    void Update()
    {
        if (!inCooldown)
            return;

       // cooldownImage.Alpha -= 1 / cooldownTime * Time.deltaTime;
    }
}
