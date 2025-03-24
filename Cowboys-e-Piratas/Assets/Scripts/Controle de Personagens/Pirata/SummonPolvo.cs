using Unity.VisualScripting;
using UnityEngine;

public class SummonPolvo : MonoBehaviour
{
    public float maxDistance;
    public GameObject areaVizualizerPrefab, areaVizualizer;
    RaycastHit raycast;
    public LayerMask ignoreLayer;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.parent.forward, out raycast, maxDistance, ~ignoreLayer))
        { 
            if(raycast.collider.gameObject.CompareTag("Ground"))
            {
                areaVizualizer.transform.position = raycast.point;
            }
        }
    }
}
