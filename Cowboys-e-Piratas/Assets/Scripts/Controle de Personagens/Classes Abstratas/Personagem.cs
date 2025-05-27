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
    public bool inputEnabled;
    public List<GameObject> weapons;
    public bool dead = false;
    public Camera playerCamera;

    [HideInInspector] public UIManager playerUI;
    [SerializeField] GameObject playerUIObject;
    
    void Start()
    {
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
        
        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            playerCamera.enabled = true;
        }
    }

    public void TakeDamage(int dano)
    {
        if (!isLocalPlayer) return;
        playerUI.UpdateHP();
        currentHp -= dano;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isLocalPlayer)
        {
            dead = true;
            playerUI.gameObject.SetActive(false); // oculta UI
            GetComponent<PlayerObjectController>().SwitchToNextAlivePlayer();
        }

        // Desativa movimentação, ações e visual
        GetComponent<Movimentacao>().enabled = false;
        foreach (var weapon in weapons)
            weapon.SetActive(false);
        anim.enabled = false;
        inputEnabled = false;

        // Oculta visualmente o jogador
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
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
