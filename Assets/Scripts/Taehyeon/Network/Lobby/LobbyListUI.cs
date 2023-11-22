using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private Button _refreshBtn;

    [SerializeField] private Transform _lobbyListContainer;
    [SerializeField] private Transform _lobbySingleTemplate;
    
    private void Awake()
    {
        _lobbySingleTemplate.gameObject.SetActive(false);
        
        _createLobbyBtn.onClick.AddListener(CreateLobbyButtonClick);
        _refreshBtn.onClick.AddListener(RefreshButtonClick);
    }

    private void Start()
    {
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> eLobbyList)
    {
        Logger.Log("Update Lobby List");
        // Destroy all lobby list, except template
        foreach (Transform child in _lobbyListContainer)       
        {
            if(child == _lobbySingleTemplate) continue;
            
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in eLobbyList)
        {
            Transform lobbySingleTransform = Instantiate(_lobbySingleTemplate, _lobbyListContainer);
            lobbySingleTransform.gameObject.SetActive(true);
            LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUI>();
            lobbyListSingleUI.UpdateLobby(lobby);
        }
        
        
    }

    private void RefreshButtonClick()
    { 
        Logger.Log("Refresh Button Clicked");
        LobbyManager.Instance.RefreshLobbyList();
    }

    private void CreateLobbyButtonClick()
    {
        CreateLobbyUI.Instance.Show();
    }

    private void Update()
    {
        _playerNameText.text = LobbyManager.Instance.PlayerName;
    }
}
