using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistance_LaserGun : LongDistanceWeaponManager, WeaponInterface, MainWeaponInterface
{
    [Space]
    [Header("LASEEGUN INFO")]
    [SerializeField] private LineRenderer _gunFirePath;
    [SerializeField] private int _gunReflectCount;

    //총알 갯수
    private int _gunBulletCount = 0;

    //총알 궤적 포인트 리스트
    private List<Vector3> l_gunPathPoints = new();

    //매 프레임마다의 총구 방향
    private Vector3 _gunFrameDirection;

    //총을 쏜 후 남은 쿨타임, 부모 클래스의 _weaponDelayTime과 비교함.
    private float _gunDelayInterval;
    private bool _isShoot = true;


    new void Start()
    {
        base.Start();
        _weaponDistance = Vector3.Distance(_weaponShotPoint.position, _weaponShotEndPoint.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) Reloading();

        _gunFrameDirection = _weaponShotEndPoint.position - _weaponShotPoint.position;
        //총알 궤적 계산
        CheckAttackRange();
        //발사 함수
        StartAttack();
    }

    void FixedUpdate()
    {
        if (_gunDelayInterval >= _weaponDelayTime) _isShoot = true;
        else _gunDelayInterval += Time.deltaTime;
    }


    //총알 궤적 계산
    public void CheckAttackRange()
    {
            l_gunPathPoints.Clear();

            float distance = _weaponDistance;
            Vector3 Input = _gunFrameDirection.normalized;
            Vector3 Beforepoint = _weaponShotPoint.position;
            Vector3 normal;
            int _gunReflectCount = 0;

            
            //distance = 0(총의 사거리) 될 때까지 reflect탐지
            while (distance > 0)
            {
                l_gunPathPoints.Add(Beforepoint);
                _weaponRay.direction = Input;
                _weaponRay.origin = Beforepoint;

                //Raycast 시도
                if (Physics.Raycast(_weaponRay, out _weaponRayHit, distance))
                {
                    //Mirror가 맞는 경우
                    if (_weaponRayHit.collider.tag == "Mirror")
                    {
                        //최대 반사 횟수에 도달한 경우
                        if (_gunReflectCount <= l_gunPathPoints.Count - 2)
                        {
                            l_gunPathPoints.Add(Beforepoint + distance * Input);
                            break;
                        }

                        //튕김이 존재하는 경우
                        else
                        {
                            _gunReflectCount++;
                            distance -= Vector3.Distance(Beforepoint, _weaponRayHit.point);
                            
                            //사거리가 0이하가 되버린 경우
                            if (distance <= 0) l_gunPathPoints.Add(Beforepoint + distance * Input);
                            
                            //아닌 경우
                            else
                            {
                                normal = _weaponRayHit.normal;
                                Input = Vector3.Reflect(Input, normal).normalized;
                                Beforepoint = _weaponRayHit.point;
                            }
                        }
                    }
                    //Mirror가 아닌 경우, 종료
                    else
                    {
                        l_gunPathPoints.Add(Beforepoint + Vector3.Distance(Beforepoint,_weaponRayHit.point)  * Input);
                        break;
                    }
                }
                //Raycast된 대상이 아예 없는 경우, 종료
                else
                {
                    l_gunPathPoints.Add(Beforepoint + distance * Input);
                    break;
                }

            }

        if (l_gunPathPoints.Count > 2)
        {
            _gunFirePath.positionCount = l_gunPathPoints.Count;
            _gunFirePath.SetPositions(l_gunPathPoints.ToArray());
            _gunFirePath.enabled = true;
        }
        else
        {
            _gunFirePath.enabled = false;
        } 
    }

    public void Reloading()
    {
        int playerLightGauge = _weaponObjectTakingManager.getCharacterCurrentGauge();
        int reloadGauge = _weaponGauge - _weaponRemainGauge;

        if (reloadGauge < playerLightGauge) updateGauge(reloadGauge);
        else updateGauge(playerLightGauge);

        print("플레이어 잔량:"+ _weaponObjectTakingManager.getCharacterCurrentGauge());

    }


    public void StartAttack()
    {
        /*
        if (Input.GetButtonDown("FireButton") && _isShoot && _gunBulletCount >= 0)
        {
            MakeNewBullet(_weaponUsingBullet, _weaponShotPoint.position, _weaponShotPoint.rotation);
            SetWeaponGauge(-_weaponAttackConsumeGauge);
            FireEffect();
            AttackReset();
        }*/
        if (_isShoot && _gunBulletCount > 0)
        {
            MakeNewBullet(_weaponUsingBullet, _weaponShotPoint.position, _weaponShotPoint.rotation);
            SetWeaponGauge(-_weaponAttackConsumeGauge);
            FireEffect();
            AttackReset();
            Debug.Log("fire");
        }
    }

    public void SetWeaponGauge(int newVal)
    {
        _weaponRemainGauge += newVal;
        _gunBulletCount -= 1;
        print(_gunBulletCount);
        SetWeaponUIGaugeBar();
    }
    public int GetWeaponGauge() => _weaponRemainGauge;

    private void AttackReset()
    {
        _gunDelayInterval = 0;
        _isShoot = false;
    }

    private void FireEffect()
    {
        SD_weaponAttackAnimation.GetValue("GunFire").Play(0);
        l_weaponSound[0].Play();
    }

    private void updateGauge(int enterGauge)
    {
        int newBulletCount = (enterGauge / _weaponAttackConsumeGauge);
        int newGauge = _weaponAttackConsumeGauge * newBulletCount;
        _gunBulletCount += newBulletCount;
        _weaponRemainGauge += newGauge;
        _weaponObjectTakingManager.SetPlayerLightGauge(-newGauge);
        print("총의 장전된 탄알 수 : " + _gunBulletCount);
        print("총의 새로 장전될 탄알 수 : " + newBulletCount);
    }


    private void MakeNewBullet(GameObject bulletObject, Vector3 bulletPosition, Quaternion bulletRotation)
    {
        GameObject newBullet = Instantiate(bulletObject);
        LongDistance_LaserBullet newBulletManager = newBullet.GetComponent<LongDistance_LaserBullet>();
        newBulletManager.SetBullet(_weaponBulletSpeed, _weaponDamage, _weaponDistance, _weaponColor, _weaponAfterImage, l_gunPathPoints, _teamColor);
        newBulletManager.SetBulletStartTransform(bulletPosition, bulletRotation);
    }

    private void SetWeaponUIGaugeBar()
    {
        Vector3 scale = l_weaponMeshRenderer[0].transform.localScale;
        scale.z = (_weaponRemainGauge / _weaponGauge);
        l_weaponMeshRenderer[0].transform.localScale = scale;
    }

    public int GetBulletCount() => _gunBulletCount;

    public int GetBulletMaxCount() => _weaponGauge / _weaponAttackConsumeGauge;
}
