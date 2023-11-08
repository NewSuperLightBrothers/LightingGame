using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BattleManager : SingletonNetwork<BattleManager>
{
    public NetworkVariable<float> curPlayTime = new(GameData.initialPlayTime);
    public MapData mapData;
    public bool isMapDateLoaded;
    
    private void Update()
    {
        if (IsServer)
        {
            if (!isMapDateLoaded) return;
            
            curPlayTime.Value -= Time.deltaTime;
        }
    }

    public void InitializeCharacterPos()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            playerObject.GetComponent<ObjectEmissionTakeManager>()._objectEmissionSystem = mapData.objectEmissionSystem;
        }
        
        if(!IsServer) return;
        
        IReadOnlyList<NetworkClient> playerList = NetworkManager.Singleton.ConnectedClientsList;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (i % 2 == 0)
            {
                // host team
                playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Red;
                playerList[i].PlayerObject.GetComponent<NCharacter>().SetPosClientRPC(mapData.respawnPoints_host[i / 2].position);
            }
            else
            {
                // client team
                playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Blue;
                playerList[i].PlayerObject.GetComponent<NCharacter>().SetPosClientRPC(mapData.respawnPoints_client[i / 2].position);
            }

        }
    }
}
