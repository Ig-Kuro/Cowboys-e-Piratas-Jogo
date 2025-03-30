using UnityEngine;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField]
    private GameObject cowboyUI,pirataUI,cowboyPlayer,pirataPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AtivarCowboy()
    {
        cowboyUI.SetActive(true);
        cowboyPlayer.SetActive(true);
    }
    public void AtivarPirata()
    {
        pirataUI.SetActive(true);
        pirataPlayer.SetActive(true);
    }
}
