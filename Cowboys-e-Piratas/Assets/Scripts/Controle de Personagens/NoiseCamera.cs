using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
//Comentei isso pq tava dando erro pra buildar
//using static UnityEditor.PlayerSettings;

public class NoiseCamera : MonoBehaviour
{
    Movimentacao movimento;
    bool busy = false;

    [Range(0, 0.1f)] public float amplitude;
    public float frequency;
    public Transform cam;
    public Transform camHolder;

    public float minStartingSpeed = 3f;

    Vector3 startingPos;
    [SerializeField]Rigidbody rb;
    void Start()
    {
        movimento = GetComponent<Movimentacao>();
        startingPos = cam.localPosition;
    }

    private void Update()
    {
        CheckSpeed();
        cam.LookAt(FocusTarget());
        ResetPos();
    }

    void CheckSpeed()
    {
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        if(!busy)
        {
            if (speed < minStartingSpeed)
            {
                PlayNoise(MakeNoise());
                return;
            }
            PlayNoise(Footstep());
        }
    }

    Vector3 Footstep()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.deltaTime * frequency) * amplitude;
        pos.x += Mathf.Sin(Time.deltaTime * frequency / 2) * amplitude * 2;
        pos.z = 0;
        return pos;
    }

    Vector3 MakeNoise()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.deltaTime * frequency/10) * amplitude / 10;
        pos.x += Mathf.Sin(Time.deltaTime * frequency / 5) * amplitude/5;
        pos.z = 0;
        return pos;
    }

    private void ResetPos()
    {
        if (cam.localPosition == startingPos)
        {
            busy = false;
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

    public void PlayNoise(Vector3 noise)
    {
        busy = true;
        cam.localPosition += noise;
    }
}
