using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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

    [Header("Store Elements")]
    [SerializeField] private TMP_Text skill1LV;
    [SerializeField] private TMP_Text skill2LV;
    [SerializeField] private TMP_Text ultLV;
    [SerializeField] private Image storeSkill1Icon;
    [SerializeField] private Image storeSkill2Icon;
    [SerializeField] private Image storeUltimateIcon;


    [Header("Skill Icons")]
    [SerializeField] private Image skill1Icon;
    [SerializeField] private Image skill2Icon;
    [SerializeField] private Image ultimateIcon;

    private bool useAmmo = false;

    void Awake() => instance = this;

    public void SetupUI(Personagem personagem, Sprite icon1, Sprite icon2, Sprite ultIcon, bool useAmmo)
    {
        player = personagem;
        skill1 = personagem.skill1;
        skill2 = personagem.skill2;
        ultimate = personagem.ult;
        this.useAmmo = useAmmo;

        skill1Icon.sprite = icon1;
        skill2Icon.sprite = icon2;
        ultimateIcon.sprite = ultIcon;
        storeSkill1Icon.sprite = icon1;
        storeSkill2Icon.sprite = icon2;
        storeUltimateIcon.sprite = ultIcon;

        life.maxValue = player.maxHp;

        ammoUI.gameObject.SetActive(useAmmo);

        UpdateLifeText();
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

    public void UpdateHP() => life.value = player.currentHp;

    public void Skill1StartCD() => StartCoroutine(StartCooldown(skill1Icon, skill1.maxCooldown));
    public void Skill2StartCD() => StartCoroutine(StartCooldown(skill2Icon, skill2.maxCooldown));

    IEnumerator StartCooldown(Image uiElement, float cooldown)
    {
        SetUIEnabled(uiElement, false);
        yield return new WaitForSeconds(cooldown);
        SetUIEnabled(uiElement, true);
    }

    public void UpdateAmmo(Gun weapon)
    {
        ammoUI.text = weapon.maxAmmo > 1000 ? "∞" : $"{weapon.currentAmmo}/{weapon.maxAmmo}";
    }

    public void UpdateLifeText()
    {
        lifeUI.text = $"{player.currentHp}/{player.maxHp}";
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
