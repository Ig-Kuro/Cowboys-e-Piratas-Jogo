using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Movimentacao))]
public abstract class Personagem : NetworkBehaviour
{

    public int currentHp, maxHp;
    public float speed;
    public float armor;
    public PersonagensAnim anim;
    public enum Classe { Pirata, Cowboy, Ninja, Viking };
    public Classe classe;

    public bool canUseSkill1, canUseSkill2, canUlt, canAttack, canReload;

    public Skill skill1, skill2;
    public Arma armaPrincipal;
    public Ultimate ult;
    public InputController input;
    public List<GameObject> weapons;

    [HideInInspector]public UIManager playerUI;
    [SerializeField] GameObject playerUIObject;

    public void TomarDano(int dano)
    {
        if(!isLocalPlayer) return;
        playerUI.UpdateHP();
        currentHp -= dano;
        if (currentHp <= 0)
        {
            SceneManager.LoadScene("Inicio");
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetGunState(int gunIndex, bool active)
    {
        RpcSetGunState(gunIndex, active); // Chama o ClientRpc no servidor
    }

    [ClientRpc]
    public void RpcSetGunState(int gunIndex, bool active)
    {
        if (gunIndex >= 0 && gunIndex < weapons.Count)
        {
            weapons[gunIndex].SetActive(active);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("Carregando UI do jogador");
        GameObject ui = Instantiate(playerUIObject);
        playerUI = ui.GetComponent<UIManager>();
        playerUI.SetupUI(this, skill1.icon, skill2.icon, ult.icon, armaPrincipal.useAmmo);
    }
}
