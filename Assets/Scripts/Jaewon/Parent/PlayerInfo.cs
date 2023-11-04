using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatInfo
{
    _playerHp = 0,
    _playerSpd,
    _playerDfn,
    _playerAtk
}

public abstract class PlayerInfo : MonoBehaviour
{
    #region 변수값
    protected GameObject _player;
    #endregion

    //플레이어의 스탯을 딕셔너리로 관리합니다. 정수값은 앞선 변수값의 열거형의 인덱스, float은 변수값을 값입니다. 기본값은 전부 100입니다.
    public Dictionary<int,float> _playerStat =  new Dictionary<int, float>();

    //현재 playerPrefab이 플레이어블 프리팹인지 불값으로 산출하는 메소드
    protected bool IsPlayablePrefab(GameObject playerPrefab) 
    {
        return true;
    }

    //GetPlayablePrefab의 값이 참이라면, 현재 이 프리팹에 카메라를 자식으로 생성합니다.

    //네트워크 방식을 몰라서 플레이어 정보를 만드는 메소드를 추상화하였습니다.
    #region 스탯 설정
    protected void InitPlayerDic()
    {
        _playerStat.Add(0, 10);
        _playerStat.Add(1, 100);
        _playerStat.Add(2, 100);
        _playerStat.Add(3, 100);
    }
    protected void SetStat(int stat, float a)
    {
        if (_playerStat.ContainsKey(stat))
        {
             _playerStat[stat] = a;
        }
    }
    #endregion
}