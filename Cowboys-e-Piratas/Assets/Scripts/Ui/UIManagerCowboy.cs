
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerCowboy : MonoBehaviour
{
    public static UIManagerCowboy instance;
    [SerializeField]
    private Personagem player;
    [SerializeField]
    private Skill skill1;
    [SerializeField]
    private Skill skill2;
    [SerializeField]
    private Ultimate ultimate;
    [SerializeField]
    GameObject EscUI,StoreUI;

    [SerializeField]
    public TMP_Text Skill1LV,Skill2LV,UltLV;
    

    public Image skill1UI,skill2UI,ultimateUI;
    public Slider life;
    public TMP_Text ammoUI,lifeUI,ultiCharge;
    void Awake()
    {
        instance=this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        life.maxValue=player.maxHp;
        lifeUI.text=player.currentHp+"/"+player.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            EscStart();
        }
        ultiCharge.text=ultimate.currentCharge+"/"+ultimate.maxCharge;
        if(ultimate.currentCharge<ultimate.maxCharge)
        {
            ultimateUI.enabled=false;
        }
        else
        {
            ultimateUI.enabled=true;
        }
    }

    public void UpdateHP()
    {
        life.maxValue=player.currentHp;
    }
    public void Skill1StartCD()
    {
        skill1UI.enabled=false;
        Invoke("Skill1OutCD",skill1.maxCooldown);
    }
    void Skill1OutCD()
    {
        skill1UI.enabled=true;
    }
    public void Skill2StartCD()
    {
        skill2UI.enabled=false;
        Invoke("Skill2OutCD",skill2.maxCooldown);
    }
    void Skill2OutCD()
    {
        skill2UI.enabled=true;
    }
    public void AttAmmo(Gun arma)
    {
        ammoUI.text=arma.currentAmmo+"/"+arma.maxAmmo;
        if(arma.maxAmmo>1000)
        {
            ammoUI.text="∞";
        }
    }
    public void AttLife()
    {
        lifeUI.text=player.currentHp+"/"+player.maxHp;
    }
    public void EscStart()
    {
        EscUI.SetActive(true);
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
        Time.timeScale=0;
    }
    public void EscEnd()
    {
        EscUI.SetActive(false);
        Cursor.visible=false;
        Cursor.lockState=CursorLockMode.Locked;
        Time.timeScale=1;
    }
    public void StoreOpen()
    {
        StoreUI.SetActive(true);
        AttStore();
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
    }
    public void StoreClose()
    {
        StoreUI.SetActive(false);
        Cursor.visible=false;
        Cursor.lockState=CursorLockMode.Locked;
    }
    public void AttStore()
    {
        Skill1LV.text= "LV: "+player.skill1.upgradeLV;
        Skill2LV.text= "LV: "+player.skill2.upgradeLV;
        UltLV.text= "LV: "+player.ult.upgradeLV;
    }
    public void EscMainMenu()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(0);
    }
}
