using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Logger = Utils.Logger;

public class BattleManager : SingletonNetwork<BattleManager>
{
    public NetworkVariable<float> curPlayTime = new();
    public int spawnTimingIdx = 0;
    public List<int> targetScoreList;
    public List<Pair<float, int>> targetGenerateTimeList = new List<Pair<float, int>>();
    
    public NetworkVariable<int> redTeamScore;
    public NetworkVariable<int> blueTeamScore;
    
    [HideInInspector] public MapData mapData;
    public bool isMapDateLoaded;

    public EObjectColorType ownerTeamColor;
    
    // for target
    public GameObject targetPrefab;
    public bool isAllTargetSpawned;

    public float nextGenTime;
    public int nextGenNumber;
    
    public int curSpawnedTargetNum = 0;
    
    
    // for character texture
    public Material redCharMaterial;
    public Material blueCharMaterial;

    public Material redStarMat;
    public Material blueStarMat;
    
    public Material redRingMat;
    public Material blueRingMat;

    public Material redWristMat;
    public Material blueWristMat;
    
    // bgm
    public AudioSource ingameBGM;
    public AudioSource resultBGM;
    
    public override void Awake()
    {
        base.Awake();

        

        redTeamScore.OnValueChanged += (prev, now) =>
        {
            BattleUIManager.Instance.UpdateScore(redTeamScore.Value, blueTeamScore.Value);
        };
        
        blueTeamScore.OnValueChanged += (prev, now) =>
        {
            BattleUIManager.Instance.UpdateScore(redTeamScore.Value, blueTeamScore.Value);
        };
        
        // target will spawn after 60, 150, 240 seconds
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 5.0f, 2));
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 30.0f, 2));
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 45.0f, 3));
        targetGenerateTimeList.Add(new Pair<float, int>(GameData.initialPlayTime - 60.0f, 3));
        
        nextGenTime = targetGenerateTimeList[spawnTimingIdx].First;
        nextGenNumber = targetGenerateTimeList[spawnTimingIdx].Second;
    }

    private void Update()
    {
        if (!IsServer) return;
        
        if (!isMapDateLoaded) return;
        
        if(Time.timeScale == 0) return;
        
        if (curPlayTime.Value <= 0)
        {
            // game end
            Logger.Log("game end");
            GameOverClientRPC();
            
            return;
        }
        curPlayTime.Value -= Time.deltaTime;

        if (!isAllTargetSpawned)
        {
            if(curPlayTime.Value <= nextGenTime)
            {
                for (int i = 0; i < nextGenNumber; i++)
                {
                    GenerateTarget(mapData.targetPosList[curSpawnedTargetNum].position);
                    curSpawnedTargetNum++;
                    if (curSpawnedTargetNum == mapData.targetPosList.Count)
                    {
                        isAllTargetSpawned = true;
                        return;
                    }
                }

                spawnTimingIdx++;

                nextGenTime = targetGenerateTimeList[spawnTimingIdx].First;
                nextGenNumber = targetGenerateTimeList[spawnTimingIdx].Second;
            }
        }
            
        
    }

    [ClientRpc]
    private void GameOverClientRPC()
    {
        Logger.Log("game over");
        BattleUIManager.Instance.ShowResult();
        Time.timeScale = 0;
        
        ingameBGM.Stop();
        resultBGM.Play();
    }

    private void GenerateTarget(Vector3 spawnPos)
    {
        if(!IsServer || targetPrefab == null) return;
        
        GameObject targetObj = Instantiate(targetPrefab, spawnPos, Quaternion.identity);
        targetObj.GetComponent<Target>().SetID(curSpawnedTargetNum);
        targetObj.GetComponent<NetworkObject>().Spawn();
        Logger.Log("targetObj spawned in " + spawnPos);
    }

    public void InitializeCharacterPos()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < playerObjects.Length; i++)
        {
            playerObjects[i].GetComponent<ObjectEmissionTakeManager>()._objectEmissionSystem = mapData.objectEmissionSystem;
            playerObjects[i].GetComponent<NCharacter>().controller.enabled = true;

            if (i % 2 == 0)
            {
                playerObjects[i].GetComponent<NCharacter>().SetCharacterTexture(redCharMaterial, redStarMat, redRingMat, redWristMat);
            }
            else
            {
                playerObjects[i].GetComponent<NCharacter>().SetCharacterTexture(blueCharMaterial, blueStarMat, blueRingMat, blueWristMat);
            }
            
        }
        
        if(!IsServer) return;

        targetScoreList = new List<int>();

        for (int i = 0; i < mapData.targetPosList.Count; i++)
        {
            targetScoreList.Add(0);
        }
        
        
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

    public void UpdateScore()
    {
        redTeamScore.Value = 0;
        blueTeamScore.Value = 0;
        
        foreach (int i in targetScoreList)
        {
            if (i > 0)
            {
                redTeamScore.Value += 1;
            }
            else if (i < 0)
            {
                blueTeamScore.Value += 1;
            }
        }
    }
}
