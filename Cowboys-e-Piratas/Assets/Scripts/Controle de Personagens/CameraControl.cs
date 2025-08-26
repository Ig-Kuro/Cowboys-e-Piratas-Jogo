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

    private float lastSendTime;
    private float sendInterval = 0.05f; // manda no máx. a cada 50ms (~20 vezes por segundo)

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

        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
        CmdRotateTorso(new Vector2(rotationX, rotationY));
        // só envia se passou do intervalo OU se mudou significativamente
        /*if (Time.time - lastSendTime > sendInterval)
        {
            lastSendTime = Time.time;
            
        }*/
    }

    private void OnTorsoRotChanged(Vector2 oldRot, Vector2 newRot)
    {
        if (torsoPersonagem != null)
        {
            Quaternion rot = Quaternion.Euler(newRot.x / 2, newRot.y, 0);
            torsoPersonagem.transform.rotation = rot;
            transform.rotation = rot;
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdRotateTorso(Vector2 rot)
    {
        torsoRot = rot;
    }
}
