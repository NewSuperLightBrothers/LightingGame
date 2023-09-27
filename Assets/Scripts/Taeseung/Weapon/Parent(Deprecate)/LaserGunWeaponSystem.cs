using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//weapon관련 최상위 클래스
public abstract class LaserGunWeaponSystem : MonoBehaviour
{
    [SerializeField] protected ELaserTeamType _team;
    [SerializeField] protected float _emissionStrength;
    protected Color _materialColor;

    private void Awake()
    {
        TakeTeamInfo();
        getTeamColor();
    }

    protected Color getTeamColor()
    {
        if (_team == ELaserTeamType.Red) _materialColor = Color.red;
        else if (_team == ELaserTeamType.Blue) _materialColor = Color.blue;
        else if (_team == ELaserTeamType.Green) _materialColor = Color.green;

        return _materialColor;
    }
    protected abstract void SetObjectTeamColor(Color color, float emissionStrength);


    //서버나 외부 데베로 부터 팀 정보를 받아올 수 있는 함수
    private void TakeTeamInfo() { }


}



//LaserGunWeaponSystem/LaserGunWeaponShootingSystem/LaserGunManager/LaserGunShootingManager
public abstract class LaserGunWeaponShootingSystem : LaserGunWeaponSystem
{
    [SerializeField] protected GunInfo _gunInfo;

    //공격사거리
    [SerializeField] protected float _distance;
    //조준점
    [SerializeField] protected Vector3 _direction;
    
    //현재 남은 탄창 게이지 측정
    protected float _currentBulletCount = 0;
    //coolTime 측정
    protected float _coolTimeInterval = 0f;
    //쏘는 중인지 아닌지 확인
    protected bool _isShoot = true;


    //총구로 부터(또는 firepoint로부터) 나가는 ray
    protected Ray _ray;
    //ray에 hit된 객체들
    protected RaycastHit[] _hits;

    protected void Start()
    {
        //사거리 초기화
        _distance = Vector3.Distance(_gunInfo.firePoint.position, _gunInfo.endPoint.position);
        print(_distance);

        //조준 방향 초기화
        _direction = _gunInfo.endPoint.position - _gunInfo.firePoint.position;
    }

    
    protected abstract void BulletFire();
    protected abstract void BulletRayCast();

}