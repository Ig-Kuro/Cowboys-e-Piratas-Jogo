using UnityEngine;

public class StoreCheck : MonoBehaviour
{
    public Collider col;
    
    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            //player.playerUI.OpenStore();
        }
    }
}
