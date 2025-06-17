using UnityEngine;

public class Blinking : MonoBehaviour
{
    public bool blinking;
    public float timeBlinking;
    // public float timeLerp;
    Material material;
    Color color;
    [ColorUsage(true, true)] public Color blinkColor1 = Color.red;
    // [ColorUsage(true, true)] public Color blinkColor2 = Color.white;
    void Start()
    {
        material = GetComponent<Renderer>().material;
        color = material.color;
    }
    void Update()
    {
        if (blinking)
        {
            /* float t = Mathf.PingPong(Time.time * timeLerp, 1f);
            material.color = Color.Lerp(blinkColor1, blinkColor2, t); */ // caso eu queira fazer piscar quando sofrer dano

            material.color = blinkColor1; // caso eu queria apenas muda a cor do material do objeto para a cor que eu quero inves de piscar
        }
    }

    public void Hit()
    {
        blinking = true;
        Invoke("StopBlinking", timeBlinking);
    }

    public void StopBlinking()
    {
        blinking = false;
        material.color = color;
    }
}
