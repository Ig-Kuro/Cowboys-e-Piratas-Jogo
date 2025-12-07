using UnityEngine;

public class QuitLobbyButton : MonoBehaviour
{
    public void OnLeaveLobbyButtonPressed()
    {
        var networkManager = Mirror.NetworkManager.singleton as CustomNetworkManager;
        if (networkManager != null)
        {
            networkManager.ResetLobby();
        }
    }

    public void OnHostButtonPressed()
    {
        var steamLobby = SteamLobby.instance;
        if (steamLobby != null)
        {
            steamLobby.HostLobby();
        }
    }
}
