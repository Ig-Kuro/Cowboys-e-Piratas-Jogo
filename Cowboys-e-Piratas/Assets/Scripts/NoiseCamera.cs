using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NoiseCamera : MonoBehaviour
{
    Movimentacao movimento;

    [Range(0, 0.5f)] public float amplitude;
    public float frequency;
    public Transform cam;
    public Transform camHolder;

    float minStartingSpeed = 3f;

    Vector3 startingPos;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movimento = GetComponent<Movimentacao>();
        startingPos = cam.localPosition;
    }

    private void Update()
    {
        CheckSpeed();
        ResetPos();
        cam.LookAt(FocusTarget());
    }

    void CheckSpeed()
    {
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        Debug.Log(speed);

        if (speed < minStartingSpeed || movimento.grounded == true)
        {
            MakeNoise();
        }
        Footstep();
    }

    Vector3 Footstep()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Sin(Time.time * frequency/2) * amplitude * 2;
        return pos;
    }

    Vector3 MakeNoise()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude/2;
        pos.x += Mathf.Sin(Time.time * frequency / 2) * amplitude;
        return pos;
    }

    private void ResetPos()
    {
        if(cam.localPosition == startingPos)
        {
            return;
        }
        cam.localPosition = Vector3.Lerp(cam.localPosition, startingPos, 1 * Time.deltaTime);
    }

    Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15f;
        return pos;
    }
}
