using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Examples.Basic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    [Header("Lobby UI")]
    public TMP_Text lobbyNameText;

    [Header("Player Data")]
    public GameObject playerListViewContent;
    public GameObject playerListItemPrefab;
    public GameObject localPlayerObject;

    [Header("Other Data")]
    public ulong currentLobbyID;
    public bool playerItemCreated = false;
    public List<PlayerListItem> playerListItems = new();
    public PlayerObjectController localPlayerObjectController;
    public PlayerSelector playerSelector;

    [Header("Ready")]
    public Button[] startGameButton;
    public TMP_Text readyButtonText;

    [Header("Manager")]
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = Mirror.NetworkManager.singleton as CustomNetworkManager;
        }
    }

    void Awake()
    {
        if(instance == null) instance = this;
    }

    public void ReadyPlayer(){
        localPlayerObjectController.ChangeReady(playerSelector.currentCharacterIndex);
        playerSelector.ChangeArrowButtons();
    }

    public void UpdateButton(){
        //Tá mudando pra todos os players
        if(localPlayerObjectController.Ready){
            readyButtonText.text = "Unready";
        }else{
            readyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady(){
        bool allReady = Manager.AreAllPlayersReady();

        foreach (Button b in startGameButton)
            b.interactable = allReady && localPlayerObjectController.PlayerIDNumber == 1;
    }

    public void UpdateLobbyName(){
        currentLobbyID = Manager.GetComponent<SteamLobby>().currentLobbyID;
        lobbyNameText.text = SteamMatchmaking.GetLobbyData((CSteamID)currentLobbyID, "name");
    }

    public void UpdatePlayerList(){
        if(!playerItemCreated) CreateHostPlayerItem();
        if(playerListItems.Count < Manager.GamePlayers.Count) CreateClientPlayerItem();
        if(playerListItems.Count > Manager.GamePlayers.Count) RemovePlayerItem();
        if(playerListItems.Count == Manager.GamePlayers.Count) UpdatePlayerItem();
    }

    public void CreateHostPlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers){
            SetupPlayerItem(player);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers){
            if(!playerListItems.Any(b => b.connectionID == player.ConnectionID)){
                SetupPlayerItem(player);
            }
        }
    }

    private void SetupPlayerItem(PlayerObjectController player){
        if(playerListViewContent != null){
            GameObject playerListItem = Instantiate(playerListItemPrefab, playerListViewContent.transform);
            PlayerListItem playerListItemController = playerListItem.GetComponent<PlayerListItem>();
            playerListItemController.playerName = player.PlayerName;
            playerListItemController.connectionID = player.ConnectionID;
            playerListItemController.playerSteamID = player.PlayerSteamID;
            playerListItemController.ready = player.Ready;
            playerListItemController.SetPlayerValues();
            playerListItems.Add(playerListItemController);
        }
    }

    public void UpdatePlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers){
            foreach(PlayerListItem playerListItem in playerListItems){
                if(playerListItem.connectionID == player.ConnectionID){
                    playerListItem.playerName = player.PlayerName;
                    playerListItem.ready = player.Ready;
                    playerListItem.SetPlayerValues();
                    if(player == localPlayerObjectController){
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem(){
        List<PlayerListItem> playerListItemsToRemove = new();
        foreach(PlayerListItem playerListItem in playerListItems){
            if(!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.connectionID)){
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if(playerListItemsToRemove.Count > 0){
            foreach(PlayerListItem playerListItem in playerListItemsToRemove){
                playerListItems.Remove(playerListItem);
                Destroy(playerListItem.gameObject);
            }
        }
    }
    
    public void LeaveLobby()
    {
        Debug.Log("Saindo do lobby Steam...");

        // Tenta sair do lobby Steam
        try
        {
            if (currentLobbyID != 0)
            {
                SteamMatchmaking.LeaveLobby((CSteamID)currentLobbyID);
                currentLobbyID = 0;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Erro ao sair do lobby Steam: " + ex.Message);
        }

        // Fecha conexões Mirror de forma segura
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost(); // host (server + client)
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient(); // apenas client
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer(); // apenas server
        }
    }

    public void StartGame(string sceneName)
    {
        localPlayerObjectController.CanStartGame(sceneName);
    }
}