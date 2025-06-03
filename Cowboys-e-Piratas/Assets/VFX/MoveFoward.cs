using UnityEngine;

public class MoveFoward : MonoBehaviour
{

    public float speed = 5f;
    public float destroyTime = 2f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
