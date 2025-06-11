using Mirror;
using UnityEngine;
public class CameraControl : NetworkBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    public int invertControl = 1;
    public InputController input;
    public float noiseStrength = 0.5f;
    public Camera cam;
    public GameObject torsoPersonagem;

    public Transform player;
    Rigidbody rb;

    public float rotationX;
    public float rotationY;

    private void Start()
    {
        if(!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        rb = player.gameObject.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if(!isLocalPlayer) return;
        float xMouse = input.MouseX() * Time.deltaTime * sensitivityX;
        float yMouse = input.MouseY() * Time.deltaTime * sensitivityY;

        rotationY += xMouse * invertControl;
        rotationX -= yMouse;

        rotationX = Mathf.Clamp(rotationX, -60, 60);

        transform.rotation = Quaternion.Euler(rotationX/2, rotationY, 0);
        if(torsoPersonagem != null) torsoPersonagem.transform.rotation = Quaternion.Euler(rotationX/2, rotationY, 0);
        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
        if(SettingsMenu.instance.cc != null)
        {
            return;
        }
        else
        {
            SettingsMenu.instance.cc = this;
        }
    }
}
