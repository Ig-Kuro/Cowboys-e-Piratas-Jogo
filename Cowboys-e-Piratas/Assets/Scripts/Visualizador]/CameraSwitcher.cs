using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public List<GameObject> camerasCenario = new List<GameObject>();
    [Header("Controle de Mouse")]
    public float sensibilidadeMouse = 200f;
    public bool controleMouseHabilitado = true;

    private int indiceCameraAtual = 0;

    void Start()
    {
        if (camerasCenario.Count == 0)
        {
            enabled = false;
            return;
        }

        if (controleMouseHabilitado)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        AtivarCamera(indiceCameraAtual);
    }

    void Update()
    {
        if (camerasCenario.Count == 0) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Cursor.lockState != CursorLockMode.Locked) return;

        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            indiceCameraAtual = (indiceCameraAtual + 1) % camerasCenario.Count;
            AtivarCamera(indiceCameraAtual);
        }

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            indiceCameraAtual--;
            if (indiceCameraAtual < 0)
            {
                indiceCameraAtual = camerasCenario.Count - 1;
            }
            AtivarCamera(indiceCameraAtual);
        }

        if (controleMouseHabilitado)
        {
            ProcessarMouseLook();
        }
    }

    void AtivarCamera(int index)
    {
        for (int i = 0; i < camerasCenario.Count; i++)
        {
            camerasCenario[i].SetActive(false);
        }

        if (index >= 0 && index < camerasCenario.Count)
        {
            GameObject novaCamera = camerasCenario[index];
            novaCamera.SetActive(true);
        }
    }

    void ProcessarMouseLook()
    {
        if (camerasCenario.Count == 0) return;

        GameObject cameraAtual = camerasCenario[indiceCameraAtual];

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * sensibilidadeMouse * Time.deltaTime;
        float mouseY = mouseDelta.y * sensibilidadeMouse * Time.deltaTime;
        if (cameraAtual.transform.parent != null)
        {
            cameraAtual.transform.parent.Rotate(Vector3.up * mouseX, Space.World);
        }
        else
        {
            cameraAtual.transform.Rotate(Vector3.up * mouseX, Space.World);
        }

        cameraAtual.transform.Rotate(Vector3.left * mouseY, Space.Self);
    }
}