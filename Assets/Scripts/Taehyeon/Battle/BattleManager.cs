using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BattleManager : SingletonNetwork<BattleManager>
{
    public NetworkVariable<float> curPlayTime = new(GameData.initialPlayTime);

    
    private void Update()
    {
        if (IsServer)
        {
            curPlayTime.Value -= Time.deltaTime;
        }
    }
}
