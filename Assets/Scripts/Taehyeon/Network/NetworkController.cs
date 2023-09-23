using Unity.Netcode;
using UnityEngine.SceneManagement;
using Logger = Utils.Logger;

public class NetworkController : SingletonNetworkPersistent<NetworkController>
{
    private NetworkVariable<int> _curPlayerNum = new(0);
    public string joinCode;

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        if (IsServer)
        {
            Logger.Log("server disconnected");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            _curPlayerNum.Value += 1;
            SetPlayerNumClientRpc(NetworkManager.Singleton.ConnectedClients.Count);
            Logger.Log($"player {_curPlayerNum.Value} connected");
            if (_curPlayerNum.Value == GameData.playerNumPerTeam * 2)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
        }
    }
    
    [ClientRpc]
    public void SetPlayerNumClientRpc(int playerNum)
    {
        GameData.currentConnectedPlayerNum = playerNum;
    }
}
