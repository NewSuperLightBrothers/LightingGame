using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using WebSocketSharp;
using Logger = Utils.Logger;

public class LobbyManager : SingletonPersistent<LobbyManager>
{
    private Lobby _hostLobby;
    private Lobby _joinedLobby;
    private float _heartbeatTimer;
    private float _lobbyUpdateTimer;
    public string PlayerName { get; private set; }
    
    // private async void Start()
    // {
    //     _playerName = "PlayerName" + UnityEngine.Random.Range(0, 99);
    //     Logger.Log("PlayerName : " + _playerName);
    //     
    //     await UnityServices.InitializeAsync();
    //     Logger.Log("LobbyManager Start");
    //
    //     AuthenticationService.Instance.SignedIn += () =>
    //     {
    //         Logger.Log("Sign in : " + AuthenticationService.Instance.PlayerId);
    //     };
    //
    //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
    // }

    private void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyPollForUpdate();
    }

    private async void HandleLobbyHeartBeat()
    {
        if (_hostLobby != null)
        {
            _heartbeatTimer -= Time.deltaTime;
            if(_heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                _heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdate()
    {
        if (_joinedLobby != null)
        {
            _lobbyUpdateTimer -= Time.deltaTime;
            if(_lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                _lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);
                _joinedLobby = lobby;
            }
        }
    }

    [TerminalCommand("CreateLobby")]
    public async void CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            if (string.IsNullOrEmpty(lobbyName))
                throw new Exception("Lobby name is empty!");

            if (maxPlayers <= 0 || maxPlayers % 2 != 0)
                throw new Exception("Max players must be an even number!");

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag", DataObject.IndexOptions.S1)}
                }
            };
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            _hostLobby = lobby;
            _joinedLobby = _hostLobby;
            
            Logger.Log("Create Lobby! : " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            
            PrintPlayers(_hostLobby);
            
        }catch(LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }catch(Exception e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("ListLobbies")]
    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    new QueryFilter(QueryFilter.FieldOptions.S1, "CaptureTheFlag", QueryFilter.OpOptions.EQ)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            
            
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            
            Logger.Log("Lobbies found : " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Logger.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        }catch(LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("JoinLobbyByCode")]
    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            
            
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            _joinedLobby = lobby;

            Logger.Log("Join lobby with code : " + lobbyCode);
            PrintPlayers(lobby);
            
        }catch(LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("QuickJoinLobby")]
    public async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
            {
                Filter = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },

            };
            
            await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) }
            }
        };
    }

    [TerminalCommand("PrintPlayers")]
    public void PrintPlayers()
    {
        PrintPlayers(_joinedLobby);
    }
    
    public void PrintPlayers(Lobby lobby)
    {
        Logger.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value);
        foreach (Player player in lobby.Players)
        {
            Logger.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    [TerminalCommand("UpdateLobbyGameMode")]
    public async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
            {
                // Don't need to update all lobby data
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            _joinedLobby = _hostLobby;
            
            PrintPlayers(_hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("UpdatePlayerName")]
    public async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            PlayerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId,
            new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject> { 
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerName) }
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("LeaveLobby")]
    public void LeaveLobby()
    {
        try
        {
            LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("KickPlayer")]
    public void KickPlayer()
    {
        try
        {
            LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, _joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("MigrateLobbyHost")]
    public async void MigrateLobbyHost()
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = _joinedLobby.Players[1].Id
            });
            _joinedLobby = _hostLobby;
            
            PrintPlayers(_hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    [TerminalCommand("DeleteLobby")]
    public void DeleteLobby()
    {
        try
        {
            LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }

    public async void Authenticate(string playerName)
    {
        PlayerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);
        
        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            Logger.Log("Sign in : " + AuthenticationService.Instance.PlayerId);

            // RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
 