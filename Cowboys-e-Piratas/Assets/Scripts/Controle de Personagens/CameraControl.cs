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

    [SyncVar(hook = nameof(OnTorsoRotChanged))]
    private Vector2 torsoRot;

    public float rotationX;
    public float rotationY;

    private void Start()
    {
        if (!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        rb = player.gameObject.GetComponent<Rigidbody>();
        if (SettingsMenu.instance.cc != null)
        {
            return;
        }
        else
        {
            SettingsMenu.instance.cc = this;
        }
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer) return;
        float xMouse = input.MouseX() * Time.deltaTime * sensitivityX;
        float yMouse = input.MouseY() * Time.deltaTime * sensitivityY;

        rotationY += xMouse * invertControl;
        rotationX -= yMouse;

        rotationX = Mathf.Clamp(rotationX, -60, 60);
        
        if (isLocalPlayer)
            CmdRotateTorso(new Vector2(rotationX, rotationY));
        
        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
    }

    private void OnTorsoRotChanged(Vector2 oldRot, Vector2 newRot)
    {
        if (torsoPersonagem != null)
        {
            torsoPersonagem.transform.rotation = Quaternion.Euler(newRot.x / 2, newRot.y, 0);
            transform.rotation = Quaternion.Euler(torsoRot.x / 2, torsoRot.y, 0);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdRotateTorso(Vector2 rot)
    {
        torsoRot = rot;
    }
}
