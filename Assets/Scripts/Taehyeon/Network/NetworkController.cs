using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Utils.Logger;

public class NetworkController : SingletonNetworkPersistent<NetworkController>
{
    public string joinCode;

    private int startPlayerNum = 2;
    
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
            if (GameData.currentConnectedPlayerNum == startPlayerNum)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
                Invoke(nameof(LoadBattleBGScene), 1f);
                
            }
        }
    }

    public void LoadBattleBGScene()
    {
        Debug.Log("LoadBattleBGScene");
        NetworkManager.Singleton.SceneManager.LoadScene("BattleBG", LoadSceneMode.Additive);
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
