using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class SummonPolvo : NetworkBehaviour
{
    public float maxDistance;
    public GameObject areaVizualizerPrefab, areaVizualizer;
    [SyncVar]
    public Vector3 visualizerPosition;
    RaycastHit raycast;
    public LayerMask ignoreLayer;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.parent.forward, out raycast, maxDistance, ~ignoreLayer))
        { 
            if(raycast.collider.gameObject.CompareTag("Ground"))
            {
                visualizerPosition = raycast.point;
                areaVizualizer.transform.position = visualizerPosition;
            }
        }
    }
}
