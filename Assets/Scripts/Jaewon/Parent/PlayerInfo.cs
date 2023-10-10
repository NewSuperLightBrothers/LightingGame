using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerInfo
{
    #region 변수값
    protected GameObject _player;

    protected float _playerHp;
    protected float _playerSpd;
    protected float _playerDfn;
    protected float _playerAtk;
    protected ELaserTeamType _team;
    #endregion

    //플레이어의 스탯을 딕셔너리로 관리합니다. 문자열은 앞선 변수값의 변수명, float은 변수값을 값입니다. 기본값은 전부 100입니다.
    protected Dictionary<string,float> _playerStat =  new Dictionary<string, float>();

    #region 초기화
    private void Awake()
    {
        DefaultStat(100,100,100,100);
        _team = ELaserTeamType.Red;
    }
    #endregion

    //네트워크 방식을 몰라서 플레이어 정보를 만드는 메소드를 추상화하였습니다.
    #region 스탯 설정
    protected abstract GameObject GetPlayerPrefab();
    protected Dictionary<string, float> DefaultStat(float hp, float spd, float dfn, float atk)
    {
        _playerStat.Add("_playerHp", hp);
        _playerStat.Add("_playerSpd", spd);
        _playerStat.Add("_playerDfn", dfn);
        _playerStat.Add("_playerAtk", atk);
        return _playerStat;
    }
    #endregion
}
