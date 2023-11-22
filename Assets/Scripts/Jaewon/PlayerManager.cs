using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerManager : PlayerInfo, IPlayerMovInfo
{
    public GameObject camArm;
    protected float _playerHp;
    protected float _playerSpd;
    protected float _playerDfn;
    protected float _playerAtk;
    public Color _teamColor;

    private void OnEnable()
    {
        _player = this.gameObject;
        InitPlayerDic();
        _teamColor = Color.red;
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
    public void Dead()
    {
        if(this._playerHp <= 0)
        {
            DestroyImmediate(this.gameObject);
        }
    }
    #endregion IPlayerMovInfo
}