using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _createBtn;
    [SerializeField] private TMP_InputField _lobbyNameInput;
    [SerializeField] private TMP_InputField _maxPlayersInput;
    
    private void Awake()
    {
        _createBtn.onClick.AddListener(() =>
        {
            Logger.Log("CreateBtn Click");
            CreateLobby();
        });
    }

    private void CreateLobby()
    {
        try
        {
            // Check lobby name validation
            if(string.IsNullOrEmpty(_lobbyNameInput.text))
                throw new Exception("Lobby name is empty");

            // Check max players validation
            if (int.TryParse(_maxPlayersInput.text, out int maxPlayers))
            {
                if(maxPlayers <= 0 || maxPlayers % 2 != 0)
                    throw new Exception("Max players is invalid");
                
                // Create lobby
                LobbyManager.Instance.CreateLobby(_lobbyNameInput.text, maxPlayers);
            }
        }catch(Exception e)
        {
            Logger.Log(e.Message);
        }
    }
}
