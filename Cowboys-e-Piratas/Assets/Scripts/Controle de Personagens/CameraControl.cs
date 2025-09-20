using Mirror;
using UnityEngine;

public class CameraControl : NetworkBehaviour
{
    [Header("Sensibilidade")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public int invertControl = 1;

    [Header("Referências")]
    public InputController input;
    public Camera cam;
    public GameObject torsoPersonagem;
    public Transform player;

    [Header("Torso")]
    [Tooltip("Quanto do pitch da câmera vai para o torso (0..1).")]
    public float torsoPitchFactor = 0.5f;
    [Tooltip("Se o bind pose tem inclinação, ajuste aqui (graus).")]
    public float torsoRotationOffset = 38f;

    [Header("Net Throttle")]
    public float sendInterval = 0.05f;       // ~20 Hz
    public float sendAngleThreshold = 0.25f; // só envia se mudar mais que isso (graus)

    private Rigidbody rb;

    [SyncVar(hook = nameof(OnTorsoRotChanged))]
    private Vector2 torsoRotNet; // (pitchX, yawY) em graus

    // estado local acumulado
    private float rotationX; // pitch
    private float rotationY; // yaw

    // cache do último valor de rede recebido
    private Vector2 torsoRotCached;

    // para limitar envios
    private float lastSendTime;
    private Vector2 lastSentRot;

    // rotação base (bind pose) do osso do torso em espaço local
    private Quaternion torsoBaseLocalRot;

    private void Start()
    {
        if (torsoPersonagem != null)
            torsoBaseLocalRot = torsoPersonagem.transform.localRotation;

        if (!isLocalPlayer) return;

        Cursor.lockState = CursorLockMode.Locked;
        rb = player.GetComponent<Rigidbody>();

        if (SettingsMenu.instance.cc == null)
            SettingsMenu.instance.cc = this;
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            float xMouse = input.MouseX() * Time.deltaTime * sensitivityX;
            float yMouse = input.MouseY() * Time.deltaTime * sensitivityY;

            rotationY += xMouse * invertControl;
            rotationX -= yMouse;
            rotationX = Mathf.Clamp(rotationX, -60f, 60f);

            // câmera local
            transform.rotation = Quaternion.Euler(rotationX * torsoPitchFactor, rotationY, 0f);

            if (rb != null)
                rb.MoveRotation(Quaternion.Euler(0f, rotationY, 0f));

            ApplyTorso(new Vector2(rotationX, rotationY));

            MaybeSendTorso(new Vector2(rotationX, rotationY));
        }
        else
        {
            ApplyTorso(torsoRotCached);
        }
    }

    private void ApplyTorso(Vector2 rot)
    {
        if (torsoPersonagem == null) return;

        // Aplicamos somente PITCH no osso do torso em espaço LOCAL.
        // O YAW já é aplicado no corpo inteiro via rb.MoveRotation.
        float pitch = rot.x * torsoPitchFactor;

        // Mantém a base do rig (bind pose) e adiciona o pitch.
        torsoPersonagem.transform.localRotation = torsoBaseLocalRot * Quaternion.Euler(pitch, 0f, 0f);
    }

    private void MaybeSendTorso(Vector2 rot)
    {
        if (Time.time - lastSendTime < sendInterval)
            return;

        // envia só se mudou o suficiente
        if (Mathf.Abs(rot.x - lastSentRot.x) < sendAngleThreshold &&
            Mathf.Abs(rot.y - lastSentRot.y) < sendAngleThreshold)
            return;

        lastSendTime = Time.time;
        lastSentRot = rot;
        CmdRotateTorso(rot);
    }

    private void OnTorsoRotChanged(Vector2 oldRot, Vector2 newRot)
    {
        // apenas atualiza o cache; a aplicação real acontece todo frame em LateUpdate
        torsoRotCached = newRot;
    }

    // Deixe o requiresAuthority padrão (true) para só o dono enviar.
    [Command]
    private void CmdRotateTorso(Vector2 rot)
    {
        torsoRotNet = rot; // dispara hook nos clientes
    }
}
