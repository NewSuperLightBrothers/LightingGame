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
    public GameObject[] allPlayers;
    public CinemachineVirtualCamera vCam;
    
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
                vCam.Follow = myCharacter.cameraFollowPoint;
                
                // UI
                myCharacter.jumpBtn = jumpBtn;
                myCharacter.fireBtn = fireBtn;
                
                myCharacter.ConnectUI();
            }
        }
    }
}
