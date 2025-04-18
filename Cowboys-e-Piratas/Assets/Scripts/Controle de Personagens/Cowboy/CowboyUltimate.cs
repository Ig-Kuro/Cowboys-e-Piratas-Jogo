using Unity.VisualScripting;
using UnityEngine;

public class CowboyUltimate : Ultimate
{
    public Cowboy cowboy;
    public float activationTime;
    float defaultFireRate;
    int defaultMaxAmmo;
    float defaulReloadTime;
    float defaultRecoil;

    private void Start()
    {
        defaulReloadTime = cowboy.primeiraPistola.reloadTime;
        defaultFireRate = cowboy.primeiraPistola.attackRate;
        defaultMaxAmmo = cowboy.primeiraPistola.maxAmmo;
        defaultRecoil = cowboy.primeiraPistola.recoil; 
    }
    public override void Action()
    {
        if (Carregado() && !usando)
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

    public override void StartUltimate()
    {
        cowboy.estado = Cowboy.state.ulting;
        cowboy.primeiraPistola.attackRate = 0.1f;
        cowboy.primeiraPistola.maxAmmo = 9999;
        cowboy.primeiraPistola.recoil = 0;
        cowboy.primeiraPistola.reloadTime = 0;
        cowboy.primeiraPistola.Reload();
        usando = true;
        Invoke("EndUltimate", duration);
    }

    public override void EndUltimate()
    {
        cowboy.estado = Cowboy.state.Normal;
        cowboy.segundaPistola.gameObject.SetActive(false);
        cowboy.armaAtual = cowboy.primeiraPistola;
        cowboy.primeiraPistola.attackRate = defaultFireRate;
        cowboy.primeiraPistola.maxAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.currentAmmo = defaultMaxAmmo;
        cowboy.primeiraPistola.reloadTime = defaulReloadTime;
        cowboy.primeiraPistola.recoil = defaultRecoil;
        cowboy.canUseSkill2 = true;
        cowboy.canUseSkill1 = true;
        usando = false;
        currentCharge = 0;
    }

    public override void CancelUltimate()
    {
        return;
    }
}
