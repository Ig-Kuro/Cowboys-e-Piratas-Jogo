using UnityEngine;

public class RotatingProjectile : MonoBehaviour
{
    public Vector3 rotationDirection;
    public float rotationSpeed;

    void FixedUpdate()
    {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
    }
}
