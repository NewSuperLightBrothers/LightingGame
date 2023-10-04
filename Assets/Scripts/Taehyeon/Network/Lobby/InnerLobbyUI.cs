using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class InnerLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private Button _leaveLobbyBtn;

    private void Awake()
    {
        _leaveLobbyBtn.onClick.AddListener(() =>
        {
            LobbyUIManager.Instance.OnLobbyLeave?.Invoke();
        });
    }

    private void OnEnable()
    {
        LobbyUIManager.Instance.OnLobbyJoin -= SetLobbyTitle;
        LobbyUIManager.Instance.OnLobbyJoin += SetLobbyTitle;
        LobbyUIManager.Instance.OnLobbyJoin -= ShowJoinedPlayerInfos;
        LobbyUIManager.Instance.OnLobbyJoin += ShowJoinedPlayerInfos;
    }

    private void OnDisable()
    {
        LobbyUIManager.Instance.OnLobbyJoin -= SetLobbyTitle;
        LobbyUIManager.Instance.OnLobbyJoin -= ShowJoinedPlayerInfos;        
    }

    private void SetLobbyTitle()
    {
        _lobbyNameText.text = LobbyManager.Instance.GetJoinedLobby()?.Name;
    }

    private void ShowJoinedPlayerInfos()
    {
        List<PlayerInfo> joinedPlayerInfos = LobbyManager.Instance.GetJoinedPlayerInfos();
        
        foreach (PlayerInfo joinedPlayerInfo in joinedPlayerInfos)
        {
            Logger.Log(joinedPlayerInfo);
        }
    }
}