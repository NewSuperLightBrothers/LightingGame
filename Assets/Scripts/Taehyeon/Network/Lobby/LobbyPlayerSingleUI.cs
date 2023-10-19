using System;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyPlayerSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private Button _kickPlayerBtn;
    
    private Player _player;

    private void Awake()
    {
        _kickPlayerBtn.onClick.AddListener(KickPlayer);    
    }
    
    public void SetKickPlayerButtonVisible(bool visible)
    {
        _kickPlayerBtn.gameObject.SetActive(visible);
    }
    
    public void UpdatePlayer(Player player)
    {
        _player = player;
        _playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
    }
    

    private void KickPlayer()
    {
        if (_player != null)
        {
            LobbyManager.Instance.KickPlayer(_player.Id);
        }
    }
}
