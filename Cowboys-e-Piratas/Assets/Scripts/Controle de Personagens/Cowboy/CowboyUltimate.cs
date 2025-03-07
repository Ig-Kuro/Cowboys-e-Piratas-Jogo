using Unity.VisualScripting;
using UnityEngine;

public class CowboyUltimate : Ultimate
{
    public Cowboy cowboy;
    public float activationTime;
    public override void Action()
    {
        if(currentCharge == maxCharge)
        {
            Invoke("StartUltimate", activationTime);
            cowboy.segundaPistola.gameObject.SetActive(true);
            cowboy.primeiraPistola.gameObject.SetActive(true);
            cowboy.estado = Cowboy.state.Normal;
            cowboy.armaAtual = cowboy.primeiraPistola;
            cowboy.rifle.gameObject.SetActive(false);
            cowboy.canUseSkill2 = false;
            cowboy.canUseSkill1 = false;

        }
    }

    public void StartUltimate()
    {
        cowboy.estado = Cowboy.state.ulting;
    }
}
