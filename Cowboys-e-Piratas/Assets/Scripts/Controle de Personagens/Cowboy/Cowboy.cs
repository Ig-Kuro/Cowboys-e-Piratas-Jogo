using Mirror;

public class Cowboy : Personagem
{
    public enum State { Normal, Lasso, Rifle, Ulting }

    public Gun rifle, primeiraPistola, segundaPistola;
    [SyncVar] public Gun armaAtual;
    [SyncVar] public State estado;

    public float buffer;
    private float timer;
    private bool attacBuffer, reloadBuffer, skill1Buffer, skill2Buffer, ultBuffer, secondaryFireBuffer;

    void Awake()
    {
        armaAtual = primeiraPistola;
        currentHp = maxHp;
        canUseSkill1 = canUseSkill2 = canUlt = canAttack = canReload = true;
    }

    void Start()
    {
        if (isLocalPlayer)
            clippingMesh.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleAttackInput();
        HandleSecondaryFireInput();
        HandleSkillInputs();
        HandleUltimateInput();
        HandleReloadInput();
    }

    private void HandleAttackInput()
    {
        if (!input.AttackInput()) return;

        if (canAttack && armaAtual.canShoot && armaAtual.currentAmmo > 0 && !armaAtual.reloading)
        {
            armaAtual.Action();
            PlayShootAnimation();
        }
        else if (canAttack && canReload && armaAtual.currentAmmo == 0 && !armaAtual.reloading)
        {
            PlayReloadAnimation();
            armaAtual.Reload();
        }
        else if (armaAtual == rifle && armaAtual.currentAmmo <= 0)
        {
            skill2.Action();
        }
    }

    private void HandleSecondaryFireInput()
    {
        if (!input.SecondaryFireInput() || estado != State.Ulting) return;

        segundaPistola.Action();
        anim.anim.SetTrigger("ShootD");
    }

    private void HandleSkillInputs()
    {
        if (input.Skill1Input() && canUseSkill1)
        {
            skill1.Action();
            //UIManagerCowboy.instance.Skill1StartCD();
        }

        if (input.Skill2Input() && canUseSkill2)
        {
            skill2.Action();
            //UIManagerCowboy.instance.Skill2StartCD();
        }
    }

    private void HandleUltimateInput()
    {
        if (input.UltimateInput() && canUlt)
        {
            ult.Action();
        }
    }

    private void HandleReloadInput()
    {
        if (!input.ReloadInput() || !canReload || armaAtual.reloading) return;

        armaAtual.Reload();

        if (armaAtual != rifle)
            PlayReloadAnimation();
    }

    private void PlayShootAnimation()
    {
        switch (estado)
        {
            case State.Ulting:
                anim.anim.SetTrigger("ShootE");
                break;
            default:
                if (armaAtual == rifle)
                    anim.anim.SetTrigger("ShootRifle");
                else
                    anim.anim.SetTrigger("Shoot");
                break;
        }
    }

    private void PlayReloadAnimation()
    {
        anim.anim.SetTrigger("Reload");
    }
}
