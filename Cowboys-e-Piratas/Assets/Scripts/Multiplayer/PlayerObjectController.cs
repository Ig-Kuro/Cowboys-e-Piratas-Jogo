using System.Collections;
using Edgegap;
using Mirror;
using Steamworks;
using UnityEngine;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar] public int characterIndex;

    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Command]
    public void CmdSetCharacterIndex(int index)
    {
        characterIndex = index;
    }

    private void PlayerReadyUpdate(bool oldReady, bool newReady)
    {
        if (isServer)
        {
            Ready = newReady;
        }
        if (isClient)
        {
            if (LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetPlayerReady(int index)
    {
        PlayerReadyUpdate(Ready, !Ready);
        characterIndex = index;
    }

    public void ChangeReady(int index)
    {
        CmdSetPlayerReady(index);
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName());
        gameObject.name = "LocalGamePlayer";
        if (LobbyController.instance != null) LobbyController.instance.localPlayerObjectController = this;
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        if (LobbyController.instance != null)
        {
            LobbyController.instance.UpdateLobbyName();
            LobbyController.instance.UpdatePlayerList();
        }
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        if (LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        PlayerNameUpdate(PlayerName, name);
    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        if (isServer)
        {
            PlayerName = newName;
        }
        if (isClient)
        {
            if (LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
        }
    }

    public void CanStartGame(string sceneName)
    {
        if (isOwned)
        {
            CmdRequestStartGame(sceneName);
        }

    }

    [Command]
    public void CmdRequestStartGame(string sceneName)
    {
        Manager.TryStartGame(this, sceneName);
    }

    public void SwitchToNextAlivePlayer()
    {
        if (!isLocalPlayer)
            return;

        Debug.Log(Manager.GamePlayers.Count);
        if (Manager.GamePlayers.Count <= 1)
        {
            LobbyController.instance.LeaveLobby();
            return;
        }

        int currentIndex = Manager.GamePlayers.IndexOf(this);
        int nextIndex = (currentIndex + 1) % Manager.GamePlayers.Count;

        for (int i = 0; i < Manager.GamePlayers.Count; i++)
        {
            var nextPlayer = Manager.GamePlayers[nextIndex].GetComponent<Personagem>();
            if (nextPlayer != this && !nextPlayer.dead)
            {
                var nextPlayerController = nextPlayer.GetComponent<Personagem>();
                nextPlayerController.playerCamera.enabled = true;
                nextPlayerController.playerCamera.GetComponent<AudioListener>().enabled = true;
                break;
            }

            nextIndex = (nextIndex + 1) % Manager.GamePlayers.Count;
        }
    }
}
