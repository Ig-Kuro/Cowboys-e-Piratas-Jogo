using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby instance;
    public GameObject hostButton;
    private NetworkManager networkManager;

    [Header("Steam Settings")]
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEnter;

    public ulong currentLobbyID;
    private const string HostAddressKey = "HostAddress";

    void Start()
    {
        if(!SteamManager.Initialized) return;
        if(instance == null) instance = this;
        networkManager = GetComponent<NetworkManager>();

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && hostButton.activeSelf)
        {
            HostLobby();
        }
    }

    public void HostLobby(){
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            hostButton.SetActive(true);
            return;
        }

        networkManager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        hostButton.SetActive(false);
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        currentLobbyID = callback.m_ulSteamIDLobby;
        if(NetworkServer.active) return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");
        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        hostButton.SetActive(false);
    }
}
