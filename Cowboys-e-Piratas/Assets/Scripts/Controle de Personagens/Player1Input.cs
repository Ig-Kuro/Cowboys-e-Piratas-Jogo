using UnityEngine;


[CreateAssetMenu(fileName = "Player1Inputs", menuName = "InputController/Player1Inputs")]
public class Player1Input : InputController
{
    public override bool AttackInput()
    {
        return Input.GetMouseButton(0);
    }
    public override bool SecondaryFireInput()
    {
        return Input.GetMouseButtonDown(1);
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

    public override bool ReloadInput()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    public override bool Skill1Input()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public override bool Skill2Input()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }


    public override bool UltimateInput()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

}
