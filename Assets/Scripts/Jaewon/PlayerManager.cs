using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerManager : PlayerInfo, IPlayerMovInfo
{
    protected override GameObject GetPlayerPrefab()
    {
        throw new System.NotImplementedException();
    }

    #region IPlayerMovInfo
    void IPlayerMovInfo.FireGun()
    {
        
    }

    void IPlayerMovInfo.FireSword()
    {
        throw new System.NotImplementedException();
    }

    void IPlayerMovInfo.GetHit()
    {
        throw new System.NotImplementedException();
    }
    #endregion IPlayerMovInfo
}
