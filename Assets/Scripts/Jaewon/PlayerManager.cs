using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerManager : PlayerInfo, IPlayerMovInfo
{
    private void OnEnable()
    {
        _player = this.gameObject;
        SetCamArm();
        InitPlayerDic();
        _teamColor = new Color(1, 0, 0, 0);
        Debug.Log(_playerStat[(int)StatInfo._playerHp]);
    }

    #region IPlayerMovInfo
    void IPlayerMovInfo.FireGun()
    {
        
    }

    void IPlayerMovInfo.FireSword()
    {
        throw new System.NotImplementedException();
    }

    void IPlayerMovInfo.GetHit(GameObject player, GameObject obj)
    {
        if(obj.GetComponent<LongDistance_LaserBullet>()._bulletColor != this._teamColor)
        {
            obj.GetComponent<LongDistance_LaserBullet>().LaserBulletToPlayer(this.GetComponent<Collider>());
            Debug.Log("피격, 현재 체력 = " + this._playerHp);
        }
    }

    #endregion IPlayerMovInfo
}