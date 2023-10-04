using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private Button _createLobbyBtn;
    [SerializeField] private GameObject _createUI;
    
    private void Awake()
    {
        _createLobbyBtn.onClick.AddListener(() =>
        {
            Logger.Log("CreateLobbyBtn Click");
            _createUI.SetActive(true);
        });
    }

    private void Update()
    {
        _playerNameText.text = LobbyManager.Instance.PlayerName;
    }
}
