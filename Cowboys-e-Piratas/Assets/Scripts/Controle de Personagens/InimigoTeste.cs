using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class InimigoTeste : MonoBehaviour
{
    public bool staggerable = true;
    public int vida;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void TomarDano(int valor)
    {
        vida -= valor;
        Debug.Log(vida);
    }
}
