using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weapon관련 최상위 클래스
public abstract class LaserGunWeaponSystem : MonoBehaviour
{
    [SerializeField]
    protected LaserTeamType Team;
    [SerializeField]
    protected float _emissionstrength;
    protected Color _materialcolor;

    private void Awake()
    {
        TakeTeamInfo();
        getTeamColor();
    }

    protected Color getTeamColor()
    {
        if (Team == LaserTeamType.Red) _materialcolor = Color.red;
        else if (Team == LaserTeamType.Blue) _materialcolor = Color.blue;
        else if (Team == LaserTeamType.Green) _materialcolor = Color.green;

        return _materialcolor;
    }
    protected abstract void SetObjectTeamColor(Color color, float emissionstrength);


    //서버나 외부 데베로 부터 팀 정보를 받아올 수 있는 함수
    private void TakeTeamInfo() { }


}



//LaserGunWeaponSystem/LaserGunWeaponShootingSystem/LaserGunManager/LaserGunShootingManager
public abstract class LaserGunWeaponShootingSystem : LaserGunWeaponSystem
{
    [SerializeField]
    protected Guninfo _guninfo;

    //공격사거리
    [SerializeField]
    protected float _distance;
    [SerializeField]
    //조준점
    protected Vector3 _direction;


    //현재 남은 탄창 게이지 측정
    protected float _currentbulletcount = 0;
    //cooltime 측정
    protected float _cooltimeinterval = 0f;
    //쏘는 중인지 아닌지 확인
    protected bool _isshoot = true;


    //총구로 부터(또는 firepoint로부터) 나가는 ray
    protected Ray _ray;
    //ray에 hit된 객체들
    protected RaycastHit[] _hits;

    protected void Start()
    {
        //사거리 초기화
        _distance = Vector3.Distance(_guninfo.firepoint.position, _guninfo.endpoint.position);
        print(_distance);

        //조준 방향 초기화
        _direction = _guninfo.endpoint.position - _guninfo.firepoint.position;
    }

    
    protected abstract void BulletFire();
    protected abstract void BulletRayCast();

}