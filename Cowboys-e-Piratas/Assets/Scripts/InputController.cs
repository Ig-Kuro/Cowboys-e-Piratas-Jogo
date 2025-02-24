using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float MoveInputX();
    public abstract float MoveInputZ();
    public abstract bool JumpInput();
    public abstract bool JumpHold();
    public abstract float MouseX();
    public abstract float MouseY();

    public abstract bool SkillInput();
    public abstract bool UltimateInput();
}
