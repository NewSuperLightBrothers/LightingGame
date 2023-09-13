using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button _hostBtn;
        [SerializeField] private Button _clientBtn;
        [SerializeField] private TMP_InputField _joinCodeInput;
        
        // [SerializeField] private TMP_Text _playersInGameText;

        private bool _isServerStarted;
        
        private void Awake()
        {
            Cursor.visible = true;
        }

        private void Start()
        {
            // Start host
            _hostBtn.onClick.AddListener(async () =>
            {
                if (RelayManager.Instance.isRelayEnabled)
                {
                    await RelayManager.Instance.SetupRelay();
                }   
                
                if (NetworkManager.Singleton.StartHost())
                {
                    Debug.Log("Host started");
                }
                else
                {
                    Debug.Log("Host failed to start");
                }
            });

            // Start client
            _clientBtn.onClick.AddListener(async () =>
            {
                if(string.IsNullOrEmpty(_joinCodeInput.text)) return;
                
                if (RelayManager.Instance.isRelayEnabled)
                {
                    await RelayManager.Instance.JoinRelay(_joinCodeInput.text);
                }


                if(NetworkManager.Singleton.StartClient()) 
                {
                    Debug.Log("Client started");   
                }
                else
                {
                    Debug.Log("Client failed to start");
                }
            });
            
            NetworkManager.Singleton.OnServerStarted += () =>
            {
                _isServerStarted = true;
            };
        }
    }