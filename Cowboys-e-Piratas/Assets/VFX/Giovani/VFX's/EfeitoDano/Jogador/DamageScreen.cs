using UnityEngine;
using UnityEngine.UI;

public class DamageScreen : MonoBehaviour
{
    [Header("Damage Screen")]
    public Color damageColor;
    public RawImage damageImage;
    public float speedSmoothing = 6.0f;
    public bool isTakingDamage;
    void Update()
    {
        if (isTakingDamage)
        {
            damageImage.color = damageColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, speedSmoothing * Time.deltaTime);
        }
        isTakingDamage = false;
    }
}
