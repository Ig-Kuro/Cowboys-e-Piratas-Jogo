using UnityEngine;


[CreateAssetMenu(fileName = "Player1Inputs", menuName = "InputController/Player1Inputs")]
public class Player1Input : InputController
{
    public override bool JumpHold()
    {
        return Input.GetButton("Jump");
    }

    public override bool JumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float MoveInputX()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float MoveInputZ()
    {
        return Input.GetAxisRaw("Vertical");
    }
}
