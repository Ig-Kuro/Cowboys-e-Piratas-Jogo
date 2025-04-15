# Cowboys-e-Piratas
## Seleção de personagens
- Entrada no lobby: um objeto com NetworkIdentity e PlayerObjectController é instanciado
- O [PlayerObjectController](https://github.com/Ig-Kuro/Cowboys-e-Piratas-Jogo/blob/pdj/Cowboys-e-Piratas/Assets/Scripts/Multiplayer/PlayerObjectController.cs) é inicializado e preenchido com informações do player no [CustomNetworkManager](https://github.com/Ig-Kuro/Cowboys-e-Piratas-Jogo/blob/main/Cowboys-e-Piratas/Assets/Scripts/Multiplayer/CustomNetworkManager.cs)
```c#
public override void OnStartServer()
{
    base.OnStartServer();
    NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
}

public override void OnClientConnect()
{
    base.OnClientConnect();

    //Criando uma message vazia só pra ter o que passar de parâmetro
    CreatePlayerMessage characterMessage = new CreatePlayerMessage
    {
        PlayerSteamID = 0
    };

    NetworkClient.Send(characterMessage);
}

public void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
{
    //Caso esteja no lobby, instancia o player vazio com insformações da conexão
    if(SceneManager.GetActiveScene().name == "Lobby")
    {
        PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab, Vector3.up, Quaternion.identity);
        gamePlayerInstance.ConnectionID = conn.connectionId;
        gamePlayerInstance.PlayerIDNumber = GamePlayers.Count + 1;
        gamePlayerInstance.PlayerSteamID = message.PlayerSteamID;

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
    }
}
```
- O personagem é selecionado através da interface que já existe
- Ao clicar em “Ready”, o player também confirma seu personagem
- Quando o host inicia o jogo, a função OnServerChangeScene do CustomNetworkManager realiza a substituição do player do lobby para o prefab do personagem.
```c#
//Verifica se não está no lobby para ativar os players
public override void OnServerChangeScene(string newSceneName)
{
    if(!newSceneName.Contains("Lobby")){
        StartCoroutine(ReplacePlayersAfterSceneLoad());
        foreach (PlayerObjectController player in GamePlayers)
        {
            if (!player.connectionToClient.isReady)
            {
                NetworkServer.SetClientReady(player.connectionToClient);
            }
            else
            {
                Debug.LogWarning($"Client {player.connectionToClient.connectionId} is not ready.");
            }
        }
    }
}

private IEnumerator ReplacePlayersAfterSceneLoad()
{
    // Espera a nova cena carregar completamente
    yield return new WaitForSeconds(.5f);

    //Percorre cada player e pega o índice do personagem selecionado para instanciar ele
    foreach (var player in GamePlayers.ToArray())
    {
        var conn = player.connectionToClient;
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
    }
}
```
