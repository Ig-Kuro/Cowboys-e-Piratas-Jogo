using UnityEngine;

public class Movimentacao : MonoBehaviour
{

    public InputController input = null;
    bool grounded = false;
    Vector3 velocity, direction, desiredVelocity;
    Rigidbody rb;
    float maxSpeedChange, acceleration;
    bool wantToJump;
    float bufferTimer;
    float startingGravity = -9.81f;
    float timerCoyote;

    [Header("Valores de pulo")]
    public float jumpStrength;
    public float jumpHeight;
    public float maxJumpVelocity;
    public float buffer;
    public float coyoteTime;
    float gravityScale;

    [Header("Valores de Movimento")]
    public float maxAirAcceleration;
    public float maxSpeed;
    public float maxAcceleration;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        direction = transform.right * input.MoveInputX() + transform.forward * input.MoveInputZ();
        desiredVelocity = new Vector3(direction.x, 0, direction.z) * maxSpeed;
        wantToJump |= input.JumpInput();

        if (input.JumpHold() && rb.linearVelocity.y > 0)
        {
            gravityScale = jumpStrength;
            if (rb.linearVelocity.y > maxJumpVelocity)
            {
                velocity.y = maxJumpVelocity - 2;
            }
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;
        if (wantToJump)
        {
            wantToJump = false;
            bufferTimer = buffer;
        }

        else if (!wantToJump && bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;
        }

        if (bufferTimer > 0)
        {
            Jump();
        }

        acceleration = grounded ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (!input.JumpHold() || rb.linearVelocity.y <= 0)
        {
            gravityScale = 1;
        } 

        if (grounded && rb.linearVelocity.y == 0)
        {
            timerCoyote = coyoteTime;
        }

        if (rb.linearVelocity.y == 0 && grounded)
        {
            timerCoyote -= Time.deltaTime;
        }
        Vector3 gravity = startingGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        if (timerCoyote > 0 && grounded)
        {
            grounded = false;
            float jumpForce = Mathf.Sqrt(-2f * startingGravity * jumpHeight);
            jumpForce = Mathf.Max(jumpForce, velocity.y, 0);
            velocity.y += jumpForce;
            bufferTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGround(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckGround(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }



    void CheckGround(Collision col)
    {
        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.gameObject.CompareTag("Ground"))
            {
                grounded = true;
                return;
            }
        }
    }

}
