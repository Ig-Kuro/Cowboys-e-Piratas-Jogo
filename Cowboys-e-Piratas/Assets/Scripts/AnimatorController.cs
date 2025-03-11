using UnityEngine;
using Mirror;

public class AnimatorController : NetworkBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isClient)
        {
            anim.SetBool("HipHop", true);
        }
        else
        {
            anim.SetBool("HipHop", false);

        }
    }
}
