using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private Button _authenticateButton;
    [SerializeField] private TMP_InputField _playerNameInputField;
    
    private void Awake()
    {
        _authenticateButton.onClick.AddListener(() =>
        {
            if(_playerNameInputField.text.IsNullOrEmpty()) return;
            
            LobbyManager.Instance.Authenticate(_playerNameInputField.text);
            LobbyUIManager.Instance.OnAuthenticate?.Invoke();
        });
        
    }
}
