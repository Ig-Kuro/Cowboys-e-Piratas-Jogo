using Mirror;
using UnityEngine;
public class CameraControl : NetworkBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    public InputController input;
    public float noiseStrength = 0.5f;
    public Camera cam;

    public Transform player;
    Rigidbody rb;

    public float rotationX;
    public float rotationY;

    private void Start()
    {
        //if(!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        rb = player.gameObject.GetComponent<Rigidbody>();
    }


    private void LateUpdate()
    {
        //if(!isLocalPlayer) return;
        float xMouse = input.MouseX() * Time.deltaTime * sensitivityX;
        float yMouse = input.MouseY() * Time.deltaTime * sensitivityY;

        rotationY += xMouse;
        rotationX -= yMouse;

        rotationX = Mathf.Clamp(rotationX, -30, 30);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
    }


}
