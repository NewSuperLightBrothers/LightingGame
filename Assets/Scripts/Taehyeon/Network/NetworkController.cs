using Unity.Netcode;
using UnityEngine.SceneManagement;
using Logger = Utils.Logger;

public class NetworkController : SingletonNetworkPersistent<NetworkController>
{
    public string joinCode;

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisConnected;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisConnected;

        if (IsServer)
        {
            Logger.Log("server disconnected");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            GameData.currentConnectedPlayerNum += 1;
            SetPlayerNumClientRpc(GameData.currentConnectedPlayerNum);
            Logger.Log($"player {clientId} connected");
            
            // Move to next scene when all players are connected
            if (GameData.currentConnectedPlayerNum == GameData.playerNumPerTeam * 2)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
        }
    }
    
    private void OnClientDisConnected(ulong clientId)
    {
        if (IsServer)
        {
            GameData.currentConnectedPlayerNum -= 1;
            SetPlayerNumClientRpc(GameData.currentConnectedPlayerNum);
            Logger.Log($"player {clientId} disConnected");
        }
    }
    
    
    [ClientRpc]
    private void SetPlayerNumClientRpc(int playerNum)
    {
        GameData.currentConnectedPlayerNum = playerNum;
    }
}
