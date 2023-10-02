using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


//무기에 대한 정보
public class LaserGunShootingManager : LaserGunWeaponShootingSystem
{
    [SerializeField] private List<MeshRenderer> _l_gunMeshRenderer;
    [SerializeField] private LineRenderer _firePath;
    [SerializeField] private int _reflectCount;

    private List<Vector3> _l_pathPoints = new();
    private Vector3 _gunDirection;



    private new void Start()
    {
        base.Start();
        _gunDirection = _direction;
        SetObjectTeamColor(_materialColor, _emissionStrength);
    }

    private void Update()
    {
        _direction = _gunInfo.endPoint.position - _gunInfo.firePoint.position;

        //총구가 흔들려서 raycast를 다시 해야하는 경우
        if (_gunDirection != _direction)
        {
            _gunDirection = _direction;
            _l_pathPoints.Clear();
            BulletRayCast();
        }
        
        //쿨타임이 돌고 터치입력이 존재했을 경우, 발사
        if (Input.GetMouseButtonDown(0) && _isShoot && _currentBulletCount - _gunInfo.usingGauge >= 0)
        {
            BulletFire();
        }

    }


    private void FixedUpdate()
    {
        if (_coolTimeInterval >= _gunInfo.coolTime) _isShoot = true;
        else _coolTimeInterval += Time.deltaTime;
        
    }

    protected override void BulletFire()
    {
        GameObject newBullet = Instantiate(_gunInfo.usingBullet);
        newBullet.transform.position = _gunInfo.firePoint.position;
        newBullet.transform.rotation = _gunInfo.firePoint.rotation;

        LaserGunBulletManager bulletManager = newBullet.GetComponent<LaserGunBulletManager>();
        bulletManager.points = _l_pathPoints.ToArray();
        bulletManager.distance = _distance;

        _gunInfo.gunAnimation.Play(0);
        _gunInfo.shootingSound.Play(0);

        _currentBulletCount -= _gunInfo.usingGauge;
        SetGaugeUIBar();

        //TestUI.testUI.setText(_currentBulletCount.ToString());

        _coolTimeInterval = 0;
        _isShoot = false;
    }


    protected override void SetObjectTeamColor(Color color, float emissionStrength)
    {
        //for (int i = 0; i < l_gunMeshRenderer.Count; i++) {
            _l_gunMeshRenderer[1].material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
        //}

        _firePath.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength-1));
    }
    protected override void BulletRayCast()
    {
        float distance = _distance;
        Vector3 Input = _direction.normalized;
        Vector3 Beforepoint = _gunInfo.firePoint.position;
        Vector3 normal;
        int reflectCount = 0;


        while (distance > 0)
        {
            _l_pathPoints.Add(Beforepoint);
            _ray.direction = Input;
            _ray.origin = Beforepoint;
            _hits = Physics.RaycastAll(_ray, distance, LayerMask.GetMask("Mirror"));

            //최대 반사 횟수에 도달한 경우
            if (_reflectCount <= _l_pathPoints.Count - 2)
            {
                _l_pathPoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                break;
            }
            //더 이상의 튕김이 없는 경우
            else if (_hits.Length <= 0)
            {
                _l_pathPoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                continue;
            }
            //튕김이 존재하는 경우
            else
            {
                reflectCount++;
                distance -= Vector3.Distance(Beforepoint, _hits[0].point);

                if (distance <= 0)
                {
                    _l_pathPoints.Add(Beforepoint + distance * Input);
                    continue;
                }
                else
                {
                    normal = _hits[0].normal;
                    Input = Vector3.Reflect(Input, normal).normalized;
                    Beforepoint = _hits[0].point;
                }
            }
        }

        _firePath.positionCount = _l_pathPoints.Count;
        _firePath.SetPositions(_l_pathPoints.ToArray());
    }


    private void SetGaugeUIBar()
    {
        Vector3 scale = _l_gunMeshRenderer[1].transform.localScale;
        scale.z = (_currentBulletCount / _gunInfo.maxGauge);
        _l_gunMeshRenderer[1].transform.localScale = scale;

    }

    public void SetGauge(float newVal)
    {
        _currentBulletCount += newVal;
        SetGaugeUIBar();
        //TestUI.testUI.setText(_currentBulletCount.ToString());
    }


    public List<Vector3> GetPathPoints()
    {
        return _l_pathPoints;
    }



}
