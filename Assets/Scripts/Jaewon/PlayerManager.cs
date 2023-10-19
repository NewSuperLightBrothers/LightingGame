using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerManager : PlayerInfo, IPlayerMovInfo
{
    [SerializeField] private GameObject _camArm;
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
        SetCamArm();
    }
    private void Start()
    {
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

    protected override void SetCamArm()
    {
        if (IsPlayablePrefab(this.gameObject))
        {
            Debug.Log("카메라 생성");
            GameObject CamArm = Instantiate(_camArm);
            CamArm.transform.parent = _player.transform;
        }
    }

    #endregion IPlayerMovInfo
}