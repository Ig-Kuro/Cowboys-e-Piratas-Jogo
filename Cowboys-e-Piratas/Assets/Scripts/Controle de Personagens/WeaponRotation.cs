using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public float smooth;
    public float rotSpeed;
    public InputController input;

    private void LateUpdate()
    {
        float xMouse = input.MouseX() * rotSpeed;
        float yMouse = input.MouseY() * rotSpeed;

        Quaternion rotx = Quaternion.AngleAxis(-yMouse, Vector3.right);
        Quaternion roty = Quaternion.AngleAxis(xMouse, Vector3.up);
        Quaternion finalRot = rotx * roty;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot, smooth * Time.deltaTime);


    }
}
