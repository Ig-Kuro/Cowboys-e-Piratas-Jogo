using UnityEngine;

public class PersonagensAnim : MonoBehaviour
{
    public Animator anim;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(Time.timeScale);
    }
    void FixedUpdate()
    {
        if(rb.linearVelocity.x == 0 || rb.linearVelocity.z == 0)
        {
            anim.SetBool("Movement", false);
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", 0);
            return;
        }
        anim.SetBool("Movement", true);
        float i = Mathf.Clamp(rb.linearVelocity.x, -1, 1);
        anim.SetFloat("X", i);
        float j = Mathf.Clamp(rb.linearVelocity.z, -1, 1);
        anim.SetFloat("Y", j);

        if(rb.linearVelocity.y > 0)
        {
            anim.SetTrigger("Pulo");
        }
    }

    public void Skill1Pirata()
    {
        anim.SetTrigger("Beber");
    }

    public void Skill2Pirata()
    {
        anim.SetTrigger("Atirar");

    }

    public void SetAttack1Pirata()
    {
        anim.SetTrigger("Ataque1");
    }
    public void SetAttack2Pirata()
    {
        anim.SetTrigger("Ataque2");
    }
    public void SetAttack3Pirata()
    {
        anim.SetTrigger("Ataque3");
    }
}
