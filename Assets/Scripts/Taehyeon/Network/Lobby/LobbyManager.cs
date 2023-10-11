using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Logger = Utils.Logger;

public class LobbyManager : SingletonPersistent<LobbyManager>
{
    private Lobby _hostLobby;
    private Lobby _joinedLobby { get; set; }
    private float _heartbeatTimer;
    private float _lobbyUpdateTimer;
    public string PlayerName { get; private set; }
    
    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;
    public class LobbyEventArgs : EventArgs {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs {
        public List<Lobby> lobbyList;
    }
    
    private void Start()
    {
        LobbyUIManager.Instance.OnLobbyLeave -= LeaveLobby;
        LobbyUIManager.Instance.OnLobbyLeave += LeaveLobby;
    }

    private void OnDestroy()
    {
        LobbyUIManager.Instance.OnLobbyLeave -= LeaveLobby;
    }

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
            
            LobbyUIManager.Instance.OnLobbyCreate?.Invoke();
            LobbyUIManager.Instance.OnLobbyJoin?.Invoke();

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

    public async void JoinLobby(Lobby lobby) {
        Player player = GetPlayer();

        _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions {
            Player = player
        });

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
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
            _hostLobby = null;
            _joinedLobby = null;
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
            Logger.Log("Sign in : " + AuthenticationService.Instance.PlayerId + " "+ playerName);

            // RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public List<PlayerInfo> GetJoinedPlayerInfos()
    {
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        try
        {
            if(_joinedLobby == null)
                throw new Exception("Joined lobby is null");

            foreach (Player joinedLobbyPlayer in _joinedLobby.Players)
            {
                PlayerInfo playerInfo = new PlayerInfo
                {
                    id = joinedLobbyPlayer.Id,
                    name = joinedLobbyPlayer.Data["PlayerName"].Value
                };
                
                playerInfos.Add(playerInfo);
            }
        }catch(Exception e)
        {
            Logger.Log(e.Message);
        }
        
        return playerInfos;
    }

    public Lobby GetJoinedLobby()
    {
        try
        {
            if(_joinedLobby == null)
                throw new Exception("Joined lobby is null");

            return _joinedLobby;
        } catch(Exception e)
        {
            Logger.Log(e.Message);
            return null;
        }
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;
            
            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };
            
            // Order by newest lobbies first
            options.Order = new List<QueryOrder>
            {
                new QueryOrder(asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListsQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            
            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs{ lobbyList = lobbyListsQueryResponse.Results});
        } catch(LobbyServiceException e)
        {
            Logger.Log(e.Message);
        }
    }
}
 