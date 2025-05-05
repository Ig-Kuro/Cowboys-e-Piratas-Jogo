using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] private Personagem player;

    void Start()
    {
        player = UIManager.instance.player;
    }

    public void BuyHealth()
    {
        player.maxHp += (int)(player.maxHp * 0.1f);
    }

    public void BuySkill1()
    {
        player.skill1.LevelUP();
    }

    public void BuySkill2()
    {
        player.skill2.LevelUP();
    }

    public void BuyUlt()
    {
        player.ult.LevelUP();
    }
}
