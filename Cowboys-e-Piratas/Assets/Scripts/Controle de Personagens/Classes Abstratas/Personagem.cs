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

    public bool canUseSkill1, canUseSkill2, canUlt, canAttack, canReload, canTakeDamage = true;

    public GameObject clippingMesh;

    public Skill skill1, skill2;
    public Sprite charPicture, hpBar;
    public BaseWeapon armaPrincipal;
    public Ultimate ult;
    public InputController input;
    public bool inputEnabled;
    public List<GameObject> weapons;
    public bool dead = false;
    public Camera playerCamera;

    [SerializeField] private Rigidbody rb;
    [SerializeField]public Movimentacao movement;
    [SerializeField]private NoiseCamera noiseCamera;
    [SerializeField] GameObject playerUIObject;

    [HideInInspector] public UIManager playerUI;



    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GameObject ui = Instantiate(playerUIObject);
        playerUI = ui.GetComponent<UIManager>();
        skill1.ci = playerUI.skill1Cooldown;
        skill2.ci = playerUI.skill2Cooldown;
        skill1.ci.cooldownTime = skill1.maxCooldown;
        skill2.ci.cooldownTime = skill2.maxCooldown;
        currentHp = maxHp;
        playerUI.SetupUI(this, skill1.icon, skill2.icon, ult.icon, armaPrincipal.useAmmo, charPicture, hpBar);
        canUseSkill1 = canUseSkill2 = canUlt = canAttack = canReload = true;
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

    public override void OnStartClient()
    {
        base.OnStartClient();
        TargetManager.instance.RegisterPlayer(this);
    }

    public void TakeDamage(int dano, Vector3 dmgDirection, bool knockBack = false, float knockBackForce = 5f)
    {
        if (!isLocalPlayer) return;
        if(!canTakeDamage) return;
        currentHp -= dano;
        playerUI.UpdateHP();
        playerUI.FlashDamage();
        if (currentHp <= 0)
        {
            CmdDie();
            return;
        }
        if (knockBack)
        {
            KnockBack(-dmgDirection, knockBackForce);
        }
    }

    public void KnockBack(Vector3 dir, float force)
    {
        dir.y = 0;
        Vector3 direction = dir + Vector3.up * 0.5f;
        if (!isLocalPlayer) return;
        rb.AddForce(direction * force, ForceMode.Impulse);
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

            TargetManager.instance.UnregisterPlayer(this);
            TargetManager.instance.NotifyPlayerDeath(this);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdHealPlayer(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
            currentHp = maxHp;

        // Atualiza a interface do jogador local
        if (isLocalPlayer && playerUI != null)
            playerUI.UpdateHP();
    }

    public void Respawn()
    {
        currentHp = maxHp / 2;
        dead = false;
        inputEnabled = true;

        TargetManager.instance.RegisterPlayer(this);

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
