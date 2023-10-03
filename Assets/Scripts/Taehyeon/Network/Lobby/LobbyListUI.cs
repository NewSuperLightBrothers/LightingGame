using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private Button _createLobbyBtn;

    private void Awake()
    {
        _createLobbyBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby("Test Lobby", 6);
        });
    }

    private void Update()
    {
        _playerNameText.text = LobbyManager.Instance.PlayerName;
    }
}
