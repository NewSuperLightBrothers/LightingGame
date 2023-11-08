using Unity.Netcode;
using UnityEngine;
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
                int pos = 0;
                int dx = 100;
                var playerList = NetworkManager.Singleton.ConnectedClientsList;

                for (int i = 0; i < playerList.Count; i++)
                {
                    if (i % 2 == 0)
                        playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Red;
                    else
                        playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Blue;
                }

                
                
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
