using UnityEngine;

public class Rota : MonoBehaviour
{
    public float velocidadeRotacao = 50f; 

   
    public Vector3 eixoRotacao = Vector3.up; 

    void Update()
    {
        transform.Rotate(eixoRotacao * velocidadeRotacao * Time.deltaTime);
    }
}