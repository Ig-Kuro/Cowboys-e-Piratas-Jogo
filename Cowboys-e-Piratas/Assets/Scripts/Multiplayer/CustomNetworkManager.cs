using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController gamePlayerPrefab;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private GameObject waveManagerPrefab;


    public List<PlayerObjectController> GamePlayers { get; } = new();

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        CreatePlayerMessage characterMessage = new CreatePlayerMessage
        {
            PlayerSteamID = 0
        };
        
        NetworkClient.Send(characterMessage);
    }

    public void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
    {
        if(SceneManager.GetActiveScene().name == "Lobby")
        {
            PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab, Vector3.up, Quaternion.identity);
            gamePlayerInstance.ConnectionID = conn.connectionId;
            gamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
            gamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, conn.connectionId);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }

    //Verifica se não está no lobby para ativar os players
    public override void OnServerChangeScene(string newSceneName)
    {
        if (!newSceneName.Contains("Lobby"))
        {
            StartCoroutine(ReplacePlayersAfterSceneLoad());
        }
    }

    //Muda para a cena do jogo através do servidor
    public void LoadScene(string sceneName){
        ServerChangeScene(sceneName);
    }

    private IEnumerator ReplacePlayersAfterSceneLoad()
    {
        // Espera a nova cena carregar completamente
        yield return new WaitForSeconds(.5f);
        GameObject waveManagerInstance = Instantiate(waveManagerPrefab);
        NetworkServer.Spawn(waveManagerInstance);

        foreach (PlayerObjectController player in GamePlayers)
        {
            NetworkConnectionToClient conn = player.connectionToClient;
            
            if (!player.connectionToClient.isReady)
            {
                NetworkServer.SetClientReady(conn);
            }
            
            int selectedIndex = player.characterIndex;
            GameObject characterInstance = Instantiate(characterPrefabs[selectedIndex]);

            // Passa dados importantes para o novo personagem
            PlayerObjectController newPlayer = characterInstance.GetComponent<PlayerObjectController>();
            newPlayer.ConnectionID = player.ConnectionID;
            newPlayer.PlayerIDNumber = player.PlayerIDNumber;
            newPlayer.PlayerSteamID = player.PlayerSteamID;
            newPlayer.PlayerName = player.PlayerName;

            // Substitui
            NetworkServer.ReplacePlayerForConnection(conn, characterInstance, ReplacePlayerOptions.KeepAuthority);

            // Remove o antigo player do tracking
            NetworkServer.Destroy(player.gameObject);

            if (WaveManager.instance != null)
            {
                WaveManager.instance.TargetUpdateGlobalUI(conn, false);
            }
        }
    }

}

    // Custom message struct for player creation
    public struct CreatePlayerMessage : NetworkMessage
    {
        public ulong PlayerSteamID;
    }