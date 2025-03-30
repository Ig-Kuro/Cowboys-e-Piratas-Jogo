
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
    GameObject EscUI;
    
    public  Gun arma;
    

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
        ammoUI.text=arma.currentAmmo+"/"+arma.maxAmmo;
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
    public void AttAmmo()
    {
        ammoUI.text=arma.currentAmmo+"/"+arma.maxAmmo;
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
    public void EscMainMenu()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(0);
    }
}
