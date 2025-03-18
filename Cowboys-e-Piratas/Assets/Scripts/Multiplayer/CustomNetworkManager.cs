using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController gamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers { get; } = new();

    //Instancia o player no lobby e seta alguns valores de conexão
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("OnServerAddPlayer " + conn.connectionId + conn.owned);
        if(SceneManager.GetActiveScene().name == "Lobby"){
            PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab, Vector3.up, Quaternion.identity);
            gamePlayerInstance.ConnectionID = conn.connectionId;
            gamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
            gamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }

    //Verifica se não está no lobby para ativar os players
    public override void OnServerChangeScene(string newSceneName)
    {
        Debug.Log("GamePlayers.Count: " + GamePlayers.Count);
        if(newSceneName != "Lobby"){
            foreach (PlayerObjectController player in GamePlayers)
            {
                player.playerModel.SetActive(true);
            }
        }
    }

    //Muda para a cena do jogo através do servidor
    public void StartGame(string sceneName){
        ServerChangeScene(sceneName);
    }
}
