using Steamworks;
using TMPro;
using UnityEngine;

public class SteamDataWarning : MonoBehaviour
{
    public TMP_Text text;
    private string template;
    void Start()
    {
        if (PlayerPrefs.GetInt("SteamWarningSeen", 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }
        // Armazena o texto original com {nome}
        template = text.text;

        // Obtém o nome do jogador na Steam
        string steamName = SteamFriends.GetPersonaName();

        // Substitui {nome} pelo nome real
        text.text = template.Replace("{nome}", steamName);
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("SteamWarningSeen", 1);
        PlayerPrefs.Save();
    }
}
