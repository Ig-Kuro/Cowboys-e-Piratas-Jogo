using UnityEngine;


[CreateAssetMenu(fileName = "Player1Inputs", menuName = "InputController/Player1Inputs")]
public class Player1Input : InputController
{
    public override bool AttackInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override bool JumpHold()
    {
        return Input.GetButton("Jump");
    }

    public override bool JumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float MouseX()
    {
        return Input.GetAxisRaw("MouseX");
    }

    public override float MouseY()
    {
        return Input.GetAxisRaw("MouseY");
    }

    public override float MoveInputX()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float MoveInputZ()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool Skill1Input()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public override bool Skill2Input()
    {
        return Input.GetMouseButtonDown(1);
    }


    public override bool UltimateInput()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

}
