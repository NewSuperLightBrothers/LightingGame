using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class NetworkUIManager : MonoBehaviour
    {
        [SerializeField] private Button _hostBtn;
        [SerializeField] private Button _clientBtn;
        [SerializeField] private Button _spawnBtn;
        
        [SerializeField] private TMP_Text _joinCodeText;
        [SerializeField] private TMP_Text _curPlayerNumText;
        [SerializeField] private TMP_InputField _joinCodeInput;
        
        private void Awake()
        {
            Cursor.visible = true;
            _curPlayerNumText.text = "000";
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
                    Logger.Log("Host started");
                }
                else
                {
                    Logger.Log("Host failed to start");
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
                    Logger.Log("Client started");   
                }
                else
                {
                    Logger.Log("Client failed to start");
                }
            });
            
            // Spawn object
            _spawnBtn.onClick.AddListener(() =>
            {
                Spawner.Instance.SpawnObjects();
            });
        }

        private void Update()
        {
            _joinCodeText.text = NetworkController.Instance.joinCode;
            _curPlayerNumText.text = GameData.currentConnectedPlayerNum.ToString();
        }
    }