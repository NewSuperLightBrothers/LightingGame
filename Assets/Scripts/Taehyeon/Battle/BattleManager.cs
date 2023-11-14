using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;

public class BattleManager : SingletonNetwork<BattleManager>
{
    public NetworkVariable<float> curPlayTime = new();
    public int nextGenerateTargetListIndex = 0;
    public NetworkList<int> targetScoreList = new NetworkList<int>(default);
    public List<Pair<float, int>> targetGenerateTimeList = new List<Pair<float, int>>();
    
    [HideInInspector] public MapData mapData;
    public bool isMapDateLoaded;

    public GameObject targetPrefab;
    public bool isAllTargetSpawned;
    
    public override void Awake()
    {
        base.Awake();

        // target will spawn after 60, 150, 240 seconds
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 10.0f, 1));
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 15.0f, 2));
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 20.0f, 2));
    }

    private void Update()
    {
        if (IsServer)
        {
            if (!isMapDateLoaded) return;
            
            curPlayTime.Value -= Time.deltaTime;

            if (!isAllTargetSpawned)
            {
                if(curPlayTime.Value <= targetGenerateTimeList[nextGenerateTargetListIndex].First)
                {
                    GenerateTarget();
                    nextGenerateTargetListIndex++;
                    if (targetGenerateTimeList.Count == nextGenerateTargetListIndex)
                        isAllTargetSpawned = true;
                }
            }
            
        }
    }

    private void GenerateTarget()
    {
        if(!IsServer || targetPrefab == null) return;
        
        GameObject targetObj = Instantiate(targetPrefab, mapData.targetPosList[nextGenerateTargetListIndex].position, Quaternion.identity);
        targetObj.GetComponent<NetworkObject>().Spawn();
        Logger.Log("targetObj spawned in " + mapData.targetPosList[nextGenerateTargetListIndex].position);
    }

    public void InitializeCharacterPos()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObject in playerObjects)
        {
            playerObject.GetComponent<ObjectEmissionTakeManager>()._objectEmissionSystem = mapData.objectEmissionSystem;
            playerObject.GetComponent<NCharacter>().controller.enabled = true;
        }
        
        if(!IsServer) return;

        // initialize curPlayTime
        curPlayTime.Value = GameData.initialPlayTime;
        Logger.Log("curPlayTime.Value = " + curPlayTime.Value);

        IReadOnlyList<NetworkClient> playerList = NetworkManager.Singleton.ConnectedClientsList;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (i % 2 == 0)
            {
                // host team
                playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Red;
                playerList[i].PlayerObject.GetComponent<NCharacter>().gun.teamColor.Value = EObjectColorType.Red;
                playerList[i].PlayerObject.GetComponent<NCharacter>().SetPosClientRPC(mapData.respawnPoints_host[i / 2].position);
            }
            else
            {
                // client team
                playerList[i].PlayerObject.GetComponent<NCharacter>().teamColor.Value = EObjectColorType.Blue;
                playerList[i].PlayerObject.GetComponent<NCharacter>().gun.teamColor.Value = EObjectColorType.Blue;
                playerList[i].PlayerObject.GetComponent<NCharacter>().SetPosClientRPC(mapData.respawnPoints_client[i / 2].position);
            }

        }
    }
}
