using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private Button _createLobbyBtn;
    
    private void Awake()
    {
        _createLobbyBtn.onClick.AddListener(CreateLobbyButtonClick);
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
