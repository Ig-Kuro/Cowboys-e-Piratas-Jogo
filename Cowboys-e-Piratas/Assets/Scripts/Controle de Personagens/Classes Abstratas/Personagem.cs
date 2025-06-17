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

    public GameObject clippingMesh;

    public Skill skill1, skill2;
    public Arma armaPrincipal;
    public Ultimate ult;
    public InputController input;
    public bool inputEnabled;
    public List<GameObject> weapons;
    public bool dead = false;
    public Camera playerCamera;

    [SerializeField] private Rigidbody rb;
    [SerializeField]private Movimentacao movement;
    [SerializeField]private NoiseCamera noiseCamera;

    [HideInInspector] public UIManager playerUI;
    [SerializeField] GameObject playerUIObject;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GameObject ui = Instantiate(playerUIObject);
        playerUI = ui.GetComponent<UIManager>();
        playerUI.SetupUI(this, skill1.icon, skill2.icon, ult.icon, armaPrincipal.useAmmo);
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            playerCamera.enabled = true;
            rb = GetComponent<Rigidbody>();
            movement = GetComponent<Movimentacao>();
            noiseCamera = GetComponent<NoiseCamera>();
        }
    }

    public void TakeDamage(int dano)
    {
        if (!isLocalPlayer) return;
        playerUI.UpdateHP();
        playerUI.FlashDamage();
        currentHp -= dano;
        if (currentHp <= 0)
        {
            CmdDie();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdDie()
    {
        if (isLocalPlayer)
        {
            dead = true;
            playerUI.gameObject.SetActive(false); // oculta UI
            // Desativa movimentação, ações e modelo do jogador
            movement.enabled = false;
            noiseCamera.enabled = false;
            rb.isKinematic = true;
            //anim.enabled = false;
            inputEnabled = false;
            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<PlayerObjectController>().SwitchToNextAlivePlayer();
        }
    }

    public void Respawn()
    {
        currentHp = maxHp / 2;
        dead = false;
        inputEnabled = true;

        // Ativa componentes de movimentação e câmera
        movement.enabled = true;
        noiseCamera.enabled = true;

        if (rb != null) rb.isKinematic = false;

        // Ativa modelo visual
        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(true);

        // Ativa UI e câmera
        if (playerUI != null)
            playerUI.gameObject.SetActive(true);

        if (playerCamera != null)
            playerCamera.enabled = true;

        if (anim != null)
            anim.enabled = true;
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

}
