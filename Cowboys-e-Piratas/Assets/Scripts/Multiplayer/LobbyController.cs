using System.Collections.Generic;
using System.Linq;
using Mirror.Examples.Basic;
//using Steamworks;
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
    public bool playerItemCreated = false;
    public List<PlayerListItem> playerListItems = new();
    public PlayerObjectController localPlayerObjectController;
    public PlayerSelector playerSelector;

    [Header("Ready")]
    public Button startGameButton;
    public TMP_Text readyButtonText;

    [Header("Manager")]
    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ReadyPlayer()
    {
        localPlayerObjectController.ChangeReady(playerSelector.currentCharacterIndex);
        playerSelector.ChangeArrowButtons();
    }

    public void UpdateButton()
    {
        if (localPlayerObjectController.Ready)
        {
            readyButtonText.text = "Unready";
        }
        else
        {
            readyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady()
    {
        bool allReady = Manager.GamePlayers.All(player => player.Ready);

        if (allReady)
        {
            startGameButton.interactable = localPlayerObjectController.PlayerIDNumber == 1;
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName()
    {
        lobbyNameText.text = "Local Lobby";
    }

    public void UpdatePlayerList()
    {
        if (!playerItemCreated) CreateHostPlayerItem();
        if (playerListItems.Count < Manager.GamePlayers.Count) CreateClientPlayerItem();
        if (playerListItems.Count > Manager.GamePlayers.Count) RemovePlayerItem();
        if (playerListItems.Count == Manager.GamePlayers.Count) UpdatePlayerItem();
    }

    public void FindLocalPlayer()
    {
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localPlayerObjectController = localPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            SetupPlayerItem(player);
        }
        playerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            if (!playerListItems.Any(b => b.connectionID == player.ConnectionID))
            {
                SetupPlayerItem(player);
            }
        }
    }

    private void SetupPlayerItem(PlayerObjectController player)
    {
        if (playerListViewContent != null)
        {
            GameObject playerListItem = Instantiate(playerListItemPrefab, playerListViewContent.transform);
            PlayerListItem playerListItemController = playerListItem.GetComponent<PlayerListItem>();

            // Define o nome do jogador como "Bora Bill" seguido do número do jogador
            playerListItemController.playerName = $"Bora Bill {player.PlayerIDNumber}";
            playerListItemController.connectionID = player.ConnectionID;
            playerListItemController.ready = player.Ready;
            playerListItemController.SetPlayerValues();
            playerListItems.Add(playerListItemController);
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.GamePlayers)
        {
            foreach (PlayerListItem playerListItem in playerListItems)
            {
                if (playerListItem.connectionID == player.ConnectionID)
                {
                    playerListItem.playerName = $"Bora Bill {player.PlayerIDNumber}";
                    playerListItem.ready = player.Ready;
                    playerListItem.SetPlayerValues();
                    if (player == localPlayerObjectController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemsToRemove = new();
        foreach (PlayerListItem playerListItem in playerListItems)
        {
            if (!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.connectionID))
            {
                Debug.Log(playerListItem);
                playerListItemsToRemove.Add(playerListItem);
            }
        }
        if (playerListItemsToRemove.Count > 0)
        {
            foreach (PlayerListItem playerListItem in playerListItemsToRemove)
            {
                playerListItems.Remove(playerListItem);
                Destroy(playerListItem.gameObject);
            }
        }
    }

    public void StartGame(string sceneName)
    {
        localPlayerObjectController.CanStartGame(sceneName);
    }
}