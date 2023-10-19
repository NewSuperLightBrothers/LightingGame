using System;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _playersText;

    private Lobby _lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobby(_lobby);
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        _lobby = lobby;

        _lobbyNameText.text = lobby.Name;
        _playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        
    }
}
