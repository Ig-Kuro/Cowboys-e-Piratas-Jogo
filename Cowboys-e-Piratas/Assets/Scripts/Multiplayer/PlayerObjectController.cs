using Mirror;
using Steamworks;
using UnityEngine;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIDNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;
    public Camera playerCamera;
    public GameObject playerModel;

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
        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
        }
        else
        {
            playerCamera.enabled = true;
            playerModel.SetActive(false);
        }
    }

    private void PlayerReadyUpdate(bool oldReady, bool newReady)
    {
        Debug.Log("isServer: " + isServer);
        Debug.Log("isClient: " + isClient);
        if (isServer)
        {
            Ready = newReady;
        }
        if (isClient)
        {
            LobbyController.instance.UpdatePlayerList();
        }
    }

    //Isso n roda no player q entra dps sem o requireAuthority false
    [Command(requiresAuthority = false)]
    private void CmdSetPlayerReady(){
        Debug.Log("CmdSetPlayerReady");
        PlayerReadyUpdate(Ready, !Ready);
    }

    public void ChangeReady(){
        CmdSetPlayerReady();
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName());
        gameObject.name = "LocalGamePlayer";
        LobbyController.instance.FindLocalPlayer();
        LobbyController.instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.instance.UpdateLobbyName();
        LobbyController.instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.instance.UpdatePlayerList();
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
            LobbyController.instance.UpdatePlayerList();
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
