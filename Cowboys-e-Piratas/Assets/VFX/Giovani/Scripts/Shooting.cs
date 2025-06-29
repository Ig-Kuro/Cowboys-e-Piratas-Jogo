using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject> ();
    public RotateToMouse rotateToMouse;

    private GameObject effectToSpawn;
    private float timeToFire = 0;
    void Start()
    {
        effectToSpawn = vfx [0];
    }
    void Update()
    {
        if(Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.A) && Time.time >= timeToFire){
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<Bullet> ().fireRate;
            SpawnVFX();
        }
    }
    void SpawnVFX(){
        GameObject vfx;
        if(firePoint != null){
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            if(rotateToMouse != null){
                vfx.transform.localRotation = rotateToMouse.GetRotation();
            }
        } else {
            Debug.Log("No Fire Point"); 
        }
    }
}
