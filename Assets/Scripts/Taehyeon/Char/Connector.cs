using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;

public class Connector : MonoBehaviour
{
    public Joystick joystick;
    public GameObject[] allPlayers;
    
    private void Awake()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        Logger.Log("Connected player num : " + allPlayers.Length);

        for (int i = 0; i < allPlayers.Length; i++)
        {
            if(allPlayers[i].GetComponent<NetworkObject>().IsOwner)
                allPlayers[i].GetComponent<NCharacter>().joystick = joystick;
        }
    }
}
