using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController gamePlayerPrefab;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private GameObject waveManagerPrefab;
    [SerializeField] private string mainMenuScene = "Inicio";

    PlayerObjectController gamePlayerInstance;
    public List<PlayerObjectController> GamePlayers { get; } = new();

    public override void Awake()
    {
        if (FindObjectsByType<NetworkManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

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
            PlayerSteamID = 0,
            //CharacterIndex = playerSelector.currentCharacterIndex,
            PlayerName = SteamFriends.GetPersonaName()
        };
        
        NetworkClient.Send(characterMessage);
    }
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        var player = conn.identity.GetComponent<PlayerObjectController>();
        if (!GamePlayers.Contains(player))
            GamePlayers.Add(player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn != null && conn.identity != null)
        {
            var player = conn.identity.GetComponent<PlayerObjectController>();

            if (player != null && GamePlayers.Contains(player))
                GamePlayers.Remove(player);
        }

        base.OnServerDisconnect(conn);

        // Verifica se GamePlayers ainda está acessível e contém algo
        if (GamePlayers != null && GamePlayers.Count == 0)
        {
            ResetLobby();
        }
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        // Evita múltiplas chamadas
        if (SteamLobby.instance != null && (CSteamID)LobbyController.instance.currentLobbyID != CSteamID.Nil)
        {
            LobbyController.instance?.LeaveLobby();
        }

        // Retorna ao menu se não for o host
        if (!NetworkServer.active && !string.IsNullOrEmpty(mainMenuScene))
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    public void ResetLobby()
    {
        /*if (NetworkServer.active && NetworkClient.isConnected)
            StopHost(); // Para host (server + client)

        else if (NetworkClient.isConnected)
            StopClient();

        else if (NetworkServer.active)
            StopServer();*/

        // Garante que sai do lobby Steam
        if (SteamLobby.instance != null && (CSteamID)LobbyController.instance.currentLobbyID != CSteamID.Nil)
        {
            LobbyController.instance?.LeaveLobby();
        }

        if (!string.IsNullOrEmpty(mainMenuScene))
            SceneManager.LoadScene(mainMenuScene);
    }

    public bool AreAllPlayersReady()
    {
        return GamePlayers.Count > 0 && GamePlayers.All(p => p.Ready);
    }

    public void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            gamePlayerInstance = Instantiate(gamePlayerPrefab, Vector3.up, Quaternion.identity);
            gamePlayerInstance.ConnectionID = conn.connectionId;
            gamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
            //gamePlayerInstance.characterIndex = message.CharacterIndex;
            gamePlayerInstance.PlayerName = message.PlayerName;
            gamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.instance.currentLobbyID, conn.connectionId);

            NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
        }
    }

    //Verifica se não está no lobby para ativar os players
    public override void OnServerSceneChanged(string newSceneName)
    {
        if (!newSceneName.Contains("Lobby"))
        {
            StartCoroutine(ReplacePlayersAfterSceneLoad());
        }
    }
    
    public void TryStartGame(PlayerObjectController requester, string sceneName)
    {
        if (requester.PlayerIDNumber == 1 && AreAllPlayersReady())
        {
            LoadScene(sceneName);
        }
    }

    //Muda para a cena do jogo através do servidor
    public void LoadScene(string sceneName)
    {
        ServerChangeScene(sceneName);
    }

    private IEnumerator ReplacePlayersAfterSceneLoad()
    {
        // Espera a nova cena carregar completamente
        yield return new WaitForSeconds(.5f);
        GameObject waveManagerInstance = Instantiate(waveManagerPrefab);
        NetworkServer.Spawn(waveManagerInstance);
        int count = 0;
        foreach (PlayerObjectController player in GamePlayers.ToArray())
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
            if(SceneManager.GetActiveScene().name == "Cowboy")
            {
                Transform spawnPoint = GameObject.Find("NetworkStartPos").transform;
                Debug.Log(spawnPoint.position);
                newPlayer.gameObject.transform.position = spawnPoint.position;
            }

            // Substitui
            NetworkServer.ReplacePlayerForConnection(conn, characterInstance, ReplacePlayerOptions.KeepAuthority);

            // Remove o antigo player do tracking
            NetworkServer.Destroy(player.gameObject);

            if (WaveManager.instance != null)
            {
                WaveManager.instance.TargetUpdateGlobalUI(conn, false);
            }
            count++;
        }
    }

}

    // Custom message struct for player creation
    public struct CreatePlayerMessage : NetworkMessage
    {
        public ulong PlayerSteamID;
        //public int CharacterIndex;
        public string PlayerName;
    }