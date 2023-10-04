using System;
using UnityEngine;
using Logger = Utils.Logger;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [SerializeField] private GameObject _authenticateUI;
    [SerializeField] private GameObject _createLobbyUI;
    [SerializeField] private GameObject _lobbyListUI;
    [SerializeField] private GameObject _innerLobbyUI;

    public Action OnAuthenticate;
    public Action OnLobbyCreate;
    public Action OnLobbyJoin;
    public Action OnLobbyLeave;

    public override void Awake()
    {
        base.Awake();

        CloseAllUI();
        _authenticateUI.SetActive(true);

        OnAuthenticate += () =>
        {
            Logger.Log("OnAuthenticate");
            CloseAllUI();
            _lobbyListUI.SetActive(true);
        };
        
        OnLobbyCreate += () =>
        {
            Logger.Log("OnLobbyCreate");
            CloseAllUI();
            _innerLobbyUI.SetActive(true);
        };
        
        OnLobbyLeave += () =>
        {
            Logger.Log("OnLobbyLeave");
            CloseAllUI();
            _lobbyListUI.SetActive(true);
        };
        
        OnLobbyJoin += () =>
        {
            Logger.Log("OnLobbyJoin");
            CloseAllUI();
            _innerLobbyUI.SetActive(true);
        };
    }

    private void CloseAllUI()
    {
        _authenticateUI.SetActive(false);
        _createLobbyUI.SetActive(false);
        _lobbyListUI.SetActive(false);
        _innerLobbyUI.SetActive(false);
    }
}
