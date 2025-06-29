using Steamworks;
using TMPro;
using UnityEngine;

public class SteamDataWarning : MonoBehaviour
{
    public TMP_Text text;
    private string template;
    void Start()
    {
        // Armazena o texto original com {nome}
        template = text.text;

        // Obtém o nome do jogador na Steam
        string steamName = SteamFriends.GetPersonaName();

        // Substitui {nome} pelo nome real
        text.text = template.Replace("{nome}", steamName);
    }
}
