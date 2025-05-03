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
    public Camera playerCamera;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null) { return manager; }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(playerCamera == null) return;
        playerCamera = GetComponentInChildren<Camera>();
        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            playerCamera.enabled = true;
        }
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
            if(LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetPlayerReady(int index){
        PlayerReadyUpdate(Ready, !Ready);
        characterIndex = index;
    }

    public void ChangeReady(int index){
        CmdSetPlayerReady(index);
    }

    public override void OnStartAuthority()
    {
        Debug.Log("OnStartAuthority called for player");
        CmdSetPlayerName(SteamFriends.GetPersonaName());
        
        //LobbyController.instance.FindLocalPlayer();
        //LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient called for player:");
        gameObject.name = "LocalGamePlayer";
        Manager.GamePlayers.Add(this);
        if (LobbyController.instance != null){
            LobbyController.instance.UpdateLobbyName();
            LobbyController.instance.UpdatePlayerList();
        }
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        if(LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        PlayerNameUpdate(PlayerName, name);
    }

    public void PlayerNameUpdate(string oldName, string newName)
    {
        if(isServer){
            PlayerName = newName;
        }
        if(isClient){
            if(LobbyController.instance != null) LobbyController.instance.UpdatePlayerList();
        }
    }

    public void CanStartGame(string sceneName)
    {
        if (isOwned)
        {
            CmdCanStartGame(sceneName);
        }

    }

    [Command]
    public void CmdCanStartGame(string sceneName){
        manager.StartGame(sceneName);
    }
}
