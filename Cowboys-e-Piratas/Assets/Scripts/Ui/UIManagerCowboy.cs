
using UnityEngine;
using UnityEngine.UI;
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
    
    public  Gun arma;
    

    public Image skill1UI,skill2UI,ultimateUI;
    public Slider vida;
    public TMP_Text muniçãoUI,vidaUI,ultiCharge;
    void Awake()
    {
        instance=this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vida.maxValue=player.maxHp;
        vidaUI.text=player.currentHp+"/"+player.maxHp;
        muniçãoUI.text=arma.currentAmmo+"/"+arma.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
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
        vida.maxValue=player.currentHp;
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
        muniçãoUI.text=arma.currentAmmo+"/"+arma.maxAmmo;
    }
}
