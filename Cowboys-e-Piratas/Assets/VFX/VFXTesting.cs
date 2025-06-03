using System.Collections.Generic;
using UnityEngine;

public class VFXTesting : MonoBehaviour
{
    public List<GameObject> vfx = new List<GameObject>();

    public GameObject effectToSpawn;


    void Start()
    {
        effectToSpawn = vfx[0];
    }

    
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.A))
        {
            SpawnVFX();
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;
        vfx = Instantiate(effectToSpawn, this.gameObject.transform.position, Quaternion.identity);
    }

}
