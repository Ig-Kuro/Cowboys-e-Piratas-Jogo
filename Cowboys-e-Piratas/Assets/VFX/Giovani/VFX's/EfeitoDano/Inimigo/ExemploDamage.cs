using UnityEngine;

public class ExemploDamage : MonoBehaviour
{
    Blinking[] blink;
    void Start()
    {
        blink = gameObject.GetComponentsInChildren<Blinking>();
    }


    void Update()
    {
        //somente para debugar e ver o efeito de dano utilizando a tecla space
        if (Input.GetKeyDown("space"))
        {
            Blink();
        }

    }

    void OllisionEnter(Collision other) // exemplo utilizando a checagem via colisao
    {
        Blink();
    }

    void Blink()
    {
        foreach (Blinking b in blink)
        {
            b.Hit();
        }
    }
}
