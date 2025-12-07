using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Personagem player;
    [SerializeField] private Skill skill1;
    [SerializeField] private Skill skill2;
    [SerializeField] private Ultimate ultimate;

    [Header("HUD Elements")]
    [SerializeField] private GameObject escUI;
    [SerializeField] private GameObject storeUI;
    [SerializeField] private Slider life;
    [SerializeField] private TMP_Text ammoUI;
    [SerializeField] private TMP_Text lifeUI;
    [SerializeField] private TMP_Text ultiCharge;
    [SerializeField] private Image hitOverlay;

    [Header("Store Elements")]
    [SerializeField] private TMP_Text skill1LV;
    [SerializeField] private TMP_Text skill2LV;
    [SerializeField] private TMP_Text ultLV;
    [SerializeField] private Image storeSkill1Icon;
    [SerializeField] private Image storeSkill2Icon;
    [SerializeField] private Image storeUltimateIcon;


    [Header("Skill Icons")]
    public CooldownIcon skill1Cooldown; 
    public CooldownIcon skill2Cooldown;
    [SerializeField] private Image skill1Icon;
    [SerializeField] private Image skill2Icon;
    [SerializeField] private Image ultimateIcon;
    public Image charIcon;
    public Image hpBar;

    private bool useAmmo = false;
    public static UIManager instance;
    private Coroutine hitRoutine;
    public void SetupUI(Personagem personagem, Sprite icon1, Sprite icon2, Sprite ultIcon, bool useAmmo, Sprite charPic, Sprite charHPBar)
    {
        player = personagem;
        skill1 = personagem.skill1;
        skill2 = personagem.skill2;
        ultimate = personagem.ult;
        this.useAmmo = useAmmo;
        skill1Icon.sprite = icon1;
        skill2Icon.sprite = icon2;
        ultimateIcon.sprite = ultIcon;
        charIcon.sprite = charPic;
        hpBar.sprite = charHPBar;
        storeSkill1Icon.sprite = icon1;
        storeSkill2Icon.sprite = icon2;
        storeUltimateIcon.sprite = ultIcon;

        life.maxValue = player.maxHp;

        ammoUI.transform.parent.gameObject.SetActive(useAmmo);

        UpdateHP();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleEscapeMenu(!escUI.activeSelf);

        if(player != null){
            ultiCharge.text = $"{ultimate.currentCharge}/{ultimate.maxCharge}";
            SetUIEnabled(ultimateIcon, ultimate.currentCharge >= ultimate.maxCharge);
        }
    }

    public void UpdateHP(){
        life.value = player.currentHp;
        lifeUI.text = $"{player.currentHp}/{player.maxHp}";
    }

    public void FlashDamage()
    {
        if (hitRoutine != null)
            StopCoroutine(hitRoutine);
        hitRoutine = StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        float duration = 0.2f;
        float alpha = 0.5f;
        Color color = new(1, 0, 0, alpha);
        hitOverlay.color = color;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fade = Mathf.Lerp(alpha, 0, timer / duration);
            hitOverlay.color = new Color(1, 0, 0, fade);
            yield return null;
        }

        hitOverlay.color = Color.clear;
        hitRoutine = null;
    }

    public void Skill1StartCD() => StartCoroutine(StartCooldown(skill1Icon, skill1.maxCooldown));
    public void Skill2StartCD() => StartCoroutine(StartCooldown(skill2Icon, skill2.maxCooldown));

    IEnumerator StartCooldown(Image uiElement, float cooldown)
    {
        SetUIEnabled(uiElement, false);
        yield return new WaitForSeconds(cooldown);
        SetUIEnabled(uiElement, true);
    }

    public void UpdateAmmo(RangedWeapon weapon)
    {
        ammoUI.text = weapon.maxAmmo > 1000 ? "∞" : $"{weapon.currentAmmo}/{weapon.maxAmmo}";
    }

    public void ToggleEscapeMenu(bool show)
    {
        escUI.SetActive(show);
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void OpenStore()
    {
        storeUI.SetActive(true);
        UpdateStoreLevels();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseStore()
    {
        storeUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateStoreLevels()
    {
        skill1LV.text = $"LV: {player.skill1.upgradeLV}";
        skill2LV.text = $"LV: {player.skill2.upgradeLV}";
        ultLV.text = $"LV: {player.ult.upgradeLV}";
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetUIEnabled(Graphic uiElement, bool state)
    {
        if (uiElement != null)
            uiElement.enabled = state;
    }
}
