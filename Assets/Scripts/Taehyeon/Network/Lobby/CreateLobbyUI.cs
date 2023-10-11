using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class CreateLobbyUI : Singleton<CreateLobbyUI>
{
    [SerializeField] private Button _createBtn;
    [SerializeField] private Button _lobbyNameBtn;
    [SerializeField] private Button _maxPlayerBtn;
    
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _maxPlayerText;
    
    private string _lobbyName;
    private int _maxPlayers;
    
    private new void Awake()
    {
        base.Awake();

        _createBtn.onClick.AddListener(() =>
        {
            Logger.Log("CreateBtn Click");
            LobbyManager.Instance.CreateLobby(
                _lobbyName,
                _maxPlayers
                );
            Hide();
        });
        
        _lobbyNameBtn.onClick.AddListener(() =>
        {
            Logger.Log("LobbyNameBtn Click");
            UI_InputWindow.Show_Static("Lobby Name", _lobbyName,
                "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
                () =>
                {
                    Logger.Log("lobby name Cancel");
                    // cancel
                },
                (string lobbyName) =>
                {
                    _lobbyName = lobbyName;
                    UpdateText();
                });
        });
        
        _maxPlayerBtn.onClick.AddListener(() =>
        {
            Logger.Log("MaxPlayerBtn Click");
            UI_InputWindow.Show_Static("Max Players", _maxPlayers,
                () =>
                {
                    Logger.Log("max player Cancel");
                    // cancel
                },
                (int maxPlayers) =>
                {
                    _maxPlayers = maxPlayers;
                    UpdateText();
                });
        });
        
        Hide();
    }

    private void UpdateText()
    {
        _lobbyNameText.text = _lobbyName;
        _maxPlayerText.text = _maxPlayers.ToString();
    }
    
    public void Show() {
        gameObject.SetActive(true);

        _lobbyName = "MyLobby";
        _maxPlayers = 4;
        
        UpdateText();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
