using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    [SerializeField]
    Personagem player;
    
    public UIManagerCowboy cowboyUI;
    public UIManagerPirata pirataUI;
    public void BuyHealth()
    {
        player.maxHp+= (player.maxHp/(10/100));
    }
    public void BuySkill1()
    {
        player.skill1.LevelUP();
        
        if(cowboyUI!=null){
            cowboyUI.AttStore();
        }
        else
        {
            pirataUI.AttStore();

        }
    }
    public void BuySkill2()
    {
        player.skill2.LevelUP();
        if(cowboyUI!=null){
            cowboyUI.AttStore();
        }
        else
        {
            pirataUI.AttStore();

        }
    }
    public void BuyUlt()
    {
        player.ult.LevelUP();
        if(cowboyUI!=null){
            cowboyUI.AttStore();
        }
        else
        {
            pirataUI.AttStore();

        }
    }
}
