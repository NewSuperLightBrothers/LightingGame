using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Logger = Utils.Logger;

public class RelayManager : Singleton<RelayManager>
{
    private const string _ENVIRONMENT_NAME = "lightgameenvironment";

    public bool isRelayEnabled =>
        _transport != null && _transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    private static UnityTransport _transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

    public async Task<RelayHostData> SetupRelay()
    {
        Logger.Log($"Relay server starting with max connections {GameData.playerNumPerTeam * 2}");
        InitializationOptions options = new InitializationOptions()
            .SetEnvironmentName(_ENVIRONMENT_NAME);
        
        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation = await Relay.Instance.CreateAllocationAsync(GameData.playerNumPerTeam * 2);

        RelayHostData relayHostData = new RelayHostData
        {
            key = allocation.Key,
            port = (ushort)allocation.RelayServer.Port,
            allocationID = allocation.AllocationId,
            allocationIDBytes = allocation.AllocationIdBytes,
            ipv4Address = allocation.RelayServer.IpV4,
            connectionData = allocation.ConnectionData
        };

        relayHostData.joinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.allocationID);

        _transport.SetRelayServerData(relayHostData.ipv4Address, relayHostData.port, relayHostData.allocationIDBytes,
            relayHostData.key, relayHostData.connectionData);
        
        NetworkController.Instance.joinCode = relayHostData.joinCode;
        
        Logger.Log($"Relay server generated a join code {relayHostData.joinCode}");

        return relayHostData;
    }

    public async Task<RelayJoinData> JoinRelay(string joinCode)
    {
        InitializationOptions options = new InitializationOptions()
            .SetEnvironmentName(_ENVIRONMENT_NAME);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        
        RelayJoinData relayJoinData = new RelayJoinData
        {
            key = allocation.Key,
            port = (ushort)allocation.RelayServer.Port,
            allocationID = allocation.AllocationId,
            allocationIDBytes = allocation.AllocationIdBytes,
            connectionData = allocation.ConnectionData,
            hostConnectionData = allocation.HostConnectionData,
            ipv4Address = allocation.RelayServer.IpV4,
            joinCode = joinCode,
        };

        _transport.SetRelayServerData(relayJoinData.ipv4Address, relayJoinData.port, relayJoinData.allocationIDBytes,
            relayJoinData.key, relayJoinData.connectionData, relayJoinData.hostConnectionData);

        Logger.Log("Client joined game with join code " + joinCode);
        return relayJoinData;
    }
}