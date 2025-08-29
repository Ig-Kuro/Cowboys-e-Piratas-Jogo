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

    public float torsoRotationOffset = 38f;

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
        float xMouse = input.MouseX() * Time.deltaTime * sensitivityX;
        float yMouse = input.MouseY() * Time.deltaTime * sensitivityY;

        rotationY += xMouse * invertControl;
        rotationX -= yMouse;

        rotationX = Mathf.Clamp(rotationX, -60, 60);

        transform.rotation = Quaternion.Euler(rotationX / 2, rotationY, 0);

        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
        CmdRotateTorso(new Vector2(rotationX, rotationY));
    }

    private void OnTorsoRotChanged(Vector2 oldRot, Vector2 newRot)
    {
        if (torsoPersonagem != null)
        {
            Quaternion torsoRotFinal = Quaternion.Euler(newRot.x / 2, newRot.y, 0);
            torsoPersonagem.transform.rotation = torsoRotFinal;
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdRotateTorso(Vector2 rot)
    {
        torsoRot = rot;
    }
}
