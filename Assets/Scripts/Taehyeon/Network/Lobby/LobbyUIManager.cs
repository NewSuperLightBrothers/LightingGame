using System;
using UnityEngine;
using Logger = Utils.Logger;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [SerializeField] private GameObject _authenticateUI;
    [SerializeField] private GameObject _createLobbyUI;
    [SerializeField] private GameObject _lobbyListUI;
    [SerializeField] private GameObject _innerLobbyUI;

    public Action OnLobbyCreate;
    public Action OnLobbyJoin;
    public Action OnLobbyLeave;

    public override void Awake()
    {
        base.Awake();
        
        _authenticateUI.SetActive(true);
    }
}
