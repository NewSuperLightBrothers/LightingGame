using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Logger = Utils.Logger;

public class LobbyManager : MonoBehaviour
{
    private Lobby _hostLobby;
    private float _heartbeatTimer;
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        Logger.Log("LobbyManager Start");

        AuthenticationService.Instance.SignedIn += () =>
        {
            Logger.Log("Sign in : " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartBeat();
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


    [TerminalCommand("CreateLobby")]
    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true
            };
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            _hostLobby = lobby;
            
            Logger.Log("Create Lobby! : " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            
        }catch(LobbyServiceException e)
        {
            Logger.Log("Create Lobby Failed! : " + e.Message);
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
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
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
                Logger.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }catch(LobbyServiceException e)
        {
            Logger.Log("List Lobbies Failed! : " + e.Message);
        }
    }

    [TerminalCommand("JoinLobby")]
    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            
            Logger.Log("Join lobby with code : " + lobbyCode);

        }catch(LobbyServiceException e)
        {
            Logger.Log("List Lobbies Failed! : " + e.Message);
        }
    }
    
}
