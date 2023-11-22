using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class Connector : MonoBehaviour
{
    public Joystick joystick;
    public Joystick LookJoystick;
    public GameObject[] allPlayers;

    public CinemachineVirtualCamera playerFollowCamera;
    public TouchZone touchZone;
    
    // UI
    public Button jumpBtn;
    public Button fireBtn;
    
    private void Awake()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i].GetComponent<NetworkObject>().IsOwner)
            {
                NCharacter myCharacter = allPlayers[i].GetComponent<NCharacter>();
                myCharacter.joystick = joystick;
                
                playerFollowCamera.Follow = myCharacter.cinemachineCameraTarget;
                myCharacter.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                
                // UI
                myCharacter.jumpBtn = jumpBtn;
                myCharacter.fireBtn = fireBtn;
                
                touchZone.touchZoneOutputEvent.AddListener(myCharacter.OnTouchLookEvent);
                myCharacter.ConnectUI();
            }
        }
    }
}
