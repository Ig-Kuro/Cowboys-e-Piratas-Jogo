using UnityEngine;

[RequireComponent(typeof(Movimentacao))]
public abstract class Personagem : MonoBehaviour
{
    public int hp;
    public float speed;
    public float armor;

    public Skill skill1, skill2;
    public Arma arma;
    public Ultimate ult;
    public InputController input;

    private void Awake()
    {
        input = GetComponent<Movimentacao>().input;
    }

    private void Update()
    {
        if(input.AttackInput())
        {
            arma.Action();
        }

        if (input.Skill1Input())
        {
            skill1.Action();
        }

        if (input.Skill2Input())
        {
            skill2.Action();
        }

        if (input.UltimateInput())
        {
           ult.Action();   
        }
    }
}
