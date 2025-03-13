using UnityEngine;

public class UltimatePirata : Ultimate
{
    public override void Action()
    {
        Debug.Log("UsePirateUlti");
    }

    public override void EndUltimate()
    {
        throw new System.NotImplementedException();
    }

    public override void StartUltimate()
    {
        throw new System.NotImplementedException();
    }
}
