using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistance_LaserGun : LongDistanceWeaponManager, WeaponInterface
{
    [Space]
    [Header("LASEEGUN INFO")]
    [SerializeField] private LineRenderer _gunFirePath;
    [SerializeField] private int _gunReflectCount;         

    //총알 궤적 포인트 리스트
    private List<Vector3> l_gunPathPoints = new();

    /// <summary>
    /// direction을 두개로 나눈 이유는 계산 비용이 큰 반사프리뷰를 조금이라도 줄이기 위해서이다.
    /// 매 프레임마다의 direction과 현재 direction이 다를 경우 반사 광선을 쏴주고 같을 경우 계산하지 않음.
    /// </summary>
    //현재 총구 방향
    private Vector3 _gunDirection;
    //매 프레임마다의 총구 방향
    private Vector3 _gunFrameDirection;

    //총을 쏜 후 남은 쿨타임, 부모 클래스의 _weaponDelayTime과 비교함.
    private float _gunDelayInterval;
    private bool _isShoot = true;


    new void Start()
    {
        base.Start();
        _weaponDistance = Vector3.Distance(_weaponShotPoint.position, _weaponShotEndPoint.position);
        _gunDirection = _weaponShotEndPoint.position - _weaponShotPoint.position;
    }

    void Update()
    {
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

    public void CheckAttackRange()
    {
            _gunDirection = _gunFrameDirection;
            l_gunPathPoints.Clear();

            float distance = _weaponDistance;
            Vector3 Input = _gunFrameDirection.normalized;
            Vector3 Beforepoint = _weaponShotPoint.position;
            Vector3 normal;
            int _gunReflectCount = 0;


            while (distance > 0)
            {
                l_gunPathPoints.Add(Beforepoint);
                _weaponRay.direction = Input;
                _weaponRay.origin = Beforepoint;
                _weaponRayHits = Physics.RaycastAll(_weaponRay, distance, LayerMask.GetMask("Mirror"));

                //최대 반사 횟수에 도달한 경우
                if (_gunReflectCount <= l_gunPathPoints.Count - 2)
                {
                    l_gunPathPoints.Add(Beforepoint + distance * Input);
                    distance = 0;
                    break;
                }
                //더 이상의 튕김이 없는 경우
                else if (_weaponRayHits.Length <= 0)
                {
                    l_gunPathPoints.Add(Beforepoint + distance * Input);
                    distance = 0;
                }
                //튕김이 존재하는 경우
                else
                {
                    _gunReflectCount++;
                    distance -= Vector3.Distance(Beforepoint, _weaponRayHits[0].point);

                    if (distance <= 0)  l_gunPathPoints.Add(Beforepoint + distance * Input);
                    else
                    {
                        normal = _weaponRayHits[0].normal;
                        Input = Vector3.Reflect(Input, normal).normalized;
                        Beforepoint = _weaponRayHits[0].point;
                    }
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

    public void StartAttack()
    {
        if (Input.GetMouseButtonDown(0) && _isShoot && _weaponRemainGauge - _weaponAttackConsumeGauge >= 0)
        {
            MakeNewBullet(_weaponUsingBullet, _weaponShotPoint.position, _weaponShotPoint.rotation);
            SetWeaponGauge(-_weaponAttackConsumeGauge);
            FireEffect();
            AttackReset();
        }
    }

    public void SetWeaponGauge(float newVal)
    {
        _weaponRemainGauge += newVal;
        SetWeaponUIGaugeBar();
    }
    public float GetWeaponGauge() => _weaponRemainGauge;
    public List<Vector3> GetPathPoints() => l_gunPathPoints;


    private void AttackReset()
    {
        _gunDelayInterval = 0;
        _isShoot = false;
    }

    private void FireEffect()
    {
        l_weaponAttackAnimation[0].Play(0);
        l_weaponSound[0].Play();
    }

    private void MakeNewBullet(GameObject bulletObject, Vector3 bulletPosition, Quaternion bulletRotation)
    {
        GameObject newBullet = Instantiate(bulletObject);
        LongDistance_LaserBullet newBulletManager = newBullet.GetComponent<LongDistance_LaserBullet>();
        newBulletManager.SetBullet(_weaponBulletSpeed, _weaponDamage, _weaponDistance, _weaponColor, l_gunPathPoints);
        newBulletManager.SetBulletStartTransform(bulletPosition, bulletRotation);
    }

    private void SetWeaponUIGaugeBar()
    {
        Vector3 scale = l_weaponMeshRenderer[0].transform.localScale;
        scale.z = (_weaponRemainGauge / _weaponGauge);
        l_weaponMeshRenderer[0].transform.localScale = scale;
    }

}
