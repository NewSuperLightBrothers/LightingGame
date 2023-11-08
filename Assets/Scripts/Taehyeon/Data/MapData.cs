using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public Transform[] respawnPoints_host;
    public Transform[] respawnPoints_client;

    public List<Transform> targetList;
    
    private void Awake()
    {
        BattleManager.Instance.mapData = this;
        BattleManager.Instance.isMapDateLoaded = true;
        BattleManager.Instance.InitializeCharacterPos();
    }
}
