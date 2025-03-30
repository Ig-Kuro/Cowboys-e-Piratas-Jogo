using UnityEngine;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField]
    private GameObject cowboyUI,pirataUI,cowboyPlayer,pirataPlayer, spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AtivarCowboy()
    {
        cowboyUI.SetActive(true);
        cowboyPlayer.SetActive(true);
        spawnManager.SetActive(true);
    }
    public void AtivarPirata()
    {
        pirataUI.SetActive(true);
        pirataPlayer.SetActive(true);
        spawnManager.SetActive(true);

    }
}
