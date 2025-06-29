using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public Camera cam;
    public float maximunLenght;

    private Ray rayMouse;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;

    void Update()
    {
        if(cam != null){
            RaycastHit hit;
            var mousePos = Input.mousePosition;
            rayMouse = cam.ScreenPointToRay(mousePos);

            if(Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximunLenght)) {
                RotateToMouseDirection(gameObject, hit.point);
            } else {
                var pos = rayMouse.GetPoint(maximunLenght);
                RotateToMouseDirection(gameObject, pos);
            }

        } else {
                Debug.Log("No Camera");
        }
    }

    void RotateToMouseDirection (GameObject obj, Vector3 destination){
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    public Quaternion GetRotation(){
        return rotation;
    }
}
