using UnityEngine;
using Mirror;

public class LocalLobby : MonoBehaviour
{
    public static LocalLobby instance;

    private CustomNetworkManager networkManager;

    void Start()
    {
        if (instance == null) instance = this;
        networkManager = GetComponent<CustomNetworkManager>();
    }

    public void HostLobby()
    {
        // Inicia o host local
        networkManager.StartHost();
        Debug.Log("Host iniciado localmente.");
    }

    public void JoinLobby(string address)
    {
        // Define o endereço do host e conecta como cliente
        networkManager.networkAddress = address;
        networkManager.StartClient();
        Debug.Log($"Tentando conectar ao host em {address}.");
    }

    public void StopLobby()
    {
        // Para o host ou cliente
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            networkManager.StopHost();
            Debug.Log("Host parado.");
        }
        else if (NetworkClient.isConnected)
        {
            networkManager.StopClient();
            Debug.Log("Cliente desconectado.");
        }
    }
}