using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapData : MonoBehaviour
{
    public Transform[] respawnPoints_host;
    public Transform[] respawnPoints_client;

    public List<Transform> targetPosList;
    
    public ObjectEmissionSystem objectEmissionSystem;
    
    private void Awake()
    {
        BattleManager.Instance.mapData = this;
        BattleManager.Instance.isMapDateLoaded = true;
        BattleManager.Instance.InitializeCharacterPos();
    }
}
