using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
//Comentei isso pq tava dando erro pra buildar
//using static UnityEditor.PlayerSettings;

public class NoiseCamera : MonoBehaviour
{
    Movimentacao movimento;

    [Range(0, 0.1f)] public float amplitude;
    public float frequency;
    public Transform cam;
    public Transform camHolder;

    public float minStartingSpeed = 3f;

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
        cam.LookAt(FocusTarget());
    }

    void CheckSpeed()
    {
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        ResetPos();

        if (speed < minStartingSpeed)
        {
            PlayNoise(MakeNoise());
            return;
        }
        PlayNoise(Footstep());
    }

    Vector3 Footstep()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Sin(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    Vector3 MakeNoise()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency/10) * amplitude / 10;
        pos.x += Mathf.Sin(Time.time * frequency / 5) * amplitude/5;
        return pos;
    }

    private void ResetPos()
    {
        if (cam.localPosition == startingPos)
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

    void PlayNoise(Vector3 noise)
    {
        cam.localPosition += noise;
    }
}
