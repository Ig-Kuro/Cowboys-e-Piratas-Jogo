using UnityEngine;

public class StoreCheck : MonoBehaviour
{
    public Collider col;
    
    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"&& Input.GetKeyDown(KeyCode.F))
        {
            if(other.gameObject.GetComponent<Cowboy>())
            {
                other.gameObject.transform.parent.gameObject.GetComponent<UIManagerCowboy>().StoreOpen();
            }
            else if(other.gameObject.GetComponent<Pirata>())
            {
                other.gameObject.transform.parent.gameObject.GetComponent<UIManagerPirata>().StoreOpen();
            }
        }
    }
}
