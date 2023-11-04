using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class TargetSpawner : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject targetPrefab;

    public bool isSpawned = false;
    private void Update()
    {
        if (IsServer)
        {
            if (BattleManager.Instance.curPlayTime.Value < 50f && !isSpawned)
            {
                GameObject spawnObj = Instantiate(targetPrefab, spawnPoints[0].position, Quaternion.identity);
                spawnObj.GetComponent<NetworkObject>().Spawn(true);
                isSpawned = true;
            }
        }
    }
}
