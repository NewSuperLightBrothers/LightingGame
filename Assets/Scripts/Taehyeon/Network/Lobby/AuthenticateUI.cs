using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private Button authenticateButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    
    private void Awake()
    {
        authenticateButton.onClick.AddListener(() =>
        {
            if(playerNameInputField.text.IsNullOrEmpty()) return;
            
            LobbyManager.Instance.Authenticate(playerNameInputField.text);
            Hide();
        });
        
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
