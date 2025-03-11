using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController gamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers { get; } = new();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name == "Lobby"){
            PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab);
            gamePlayerInstance.ConnectionID = conn.connectionId;
            gamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
            gamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, GamePlayers.Count);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }

    public void StartGame(string sceneName){
        ServerChangeScene(sceneName);
    }
}
