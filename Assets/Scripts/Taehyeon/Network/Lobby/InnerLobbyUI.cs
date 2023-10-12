using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class InnerLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _playerCountText;

    [SerializeField] private Transform _playerSingleTemplate;
    [SerializeField] private Transform _playerContainer;
    
    [SerializeField] private Button _leaveLobbyBtn;
    [SerializeField] private Button _StartGameBtn;

    private void Awake()
    {
        _playerSingleTemplate.gameObject.SetActive(false);
        
        _leaveLobbyBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
        });
        
        _StartGameBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.StartGame();
        });
    }

    private void Start()
    {
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Transform playerSingleTransform = Instantiate(_playerSingleTemplate, _playerContainer);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
            
            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() && 
                player.Id != AuthenticationService.Instance.PlayerId); // Don't allow kick self
            
            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        if (!LobbyManager.Instance.IsLobbyHost())
        {
            _StartGameBtn.gameObject.SetActive(false);
        }

        _lobbyNameText.text = lobby.Name;
        _playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void ClearLobby()
    {
        foreach (Transform child in _playerContainer)
        {
            if(child == _playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
}