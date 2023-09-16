using Unity.Netcode;
using UnityEngine.SceneManagement;
using Logger = Utils.Logger;

public class NetworkController : SingletonNetworkPersistent<NetworkController>
{
    private NetworkVariable<int> _curPlayerNum = new NetworkVariable<int>(0);
    private const int _MAX_PLAYER_NUM = 2;

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
            Logger.Log($"player {_curPlayerNum.Value} connected");
            if (_curPlayerNum.Value == _MAX_PLAYER_NUM)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            }
            
        }
    }
}
