using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//무기에 대한 정보
public class LaserGunShootingManager : LaserGunWeaponShootingSystem
{
    [SerializeField]
    private List<MeshRenderer> L_Gunmeshrenderer;
    [SerializeField]
    private LineRenderer FirePath;
    [SerializeField]
    private int _reflectcount;

    private List<Vector3> l_pathpoints = new();
    private Vector3 _gundirection;



    private new void Start()
    {
        base.Start();
        _gundirection = _direction;
        SetObjectTeamColor(_materialcolor, _emissionstrength);
    }

    private void Update()
    {
        _direction = _guninfo.endpoint.position - _guninfo.firepoint.position;

        //총구가 흔들려서 raycast를 다시 해야하는 경우
        if (_gundirection != _direction)
        {
            _gundirection = _direction;
            l_pathpoints.Clear();
            BulletRayCast();
        }
        //쿨타임이 돌고 터치입력이 존재했을 경우, 발사
        if (Input.GetMouseButtonDown(0) && _isshoot && _currentbulletcount - _guninfo.usinggauge >= 0)
        {
            BulletFire();
        }

    }


    private void FixedUpdate()
    {
        if (_cooltimeinterval >= _guninfo.Cooltime) _isshoot = true;
        else _cooltimeinterval += Time.deltaTime;
        
    }

    protected override void BulletFire()
    {
        GameObject newbullet = Instantiate(_guninfo.usingbullet);
        newbullet.transform.position = _guninfo.firepoint.position;
        newbullet.transform.rotation = _guninfo.firepoint.rotation;

        LaserGunBulletManager bulletmanager = newbullet.GetComponent<LaserGunBulletManager>();
        bulletmanager.points = l_pathpoints.ToArray();
        bulletmanager.distance = _distance;

        _guninfo.gunanimation.Play(0);
        _guninfo.shootingsound.Play(0);

        _currentbulletcount -= _guninfo.usinggauge;
        TestUI.testUI.setText(_currentbulletcount.ToString());

        _cooltimeinterval = 0;
        _isshoot = false;
    }


    protected override void SetObjectTeamColor(Color color, float emissionstrength)
    {
        for (int i = 0; i < L_Gunmeshrenderer.Count; i++) {
            L_Gunmeshrenderer[i].material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionstrength));
        }

        FirePath.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionstrength-1));
    }
    protected override void BulletRayCast()
    {
        float distance = _distance;
        Vector3 Input = _direction.normalized;
        Vector3 Beforepoint = _guninfo.firepoint.position;
        Vector3 normal;
        int reflectcount = 0;


        while (distance > 0)
        {
            l_pathpoints.Add(Beforepoint);
            _ray.direction = Input;
            _ray.origin = Beforepoint;
            _hits = Physics.RaycastAll(_ray, distance, LayerMask.GetMask("Mirror"));

            //최대 반사 횟수에 도달한 경우
            if (_reflectcount <= l_pathpoints.Count - 2)
            {
                l_pathpoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                break;
            }
            //더 이상의 튕김이 없는 경우
            else if (_hits.Length <= 0)
            {
                l_pathpoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                continue;
            }
            //튕김이 존재하는 경우
            else
            {
                reflectcount++;
                distance -= Vector3.Distance(Beforepoint, _hits[0].point);

                if (distance <= 0)
                {
                    l_pathpoints.Add(Beforepoint + distance * Input);
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

        FirePath.positionCount = l_pathpoints.Count;
        FirePath.SetPositions(l_pathpoints.ToArray());
    }

    public void SetGauge(float newval)
    {
        _currentbulletcount += newval;
        TestUI.testUI.setText(_currentbulletcount.ToString());
    }

    public List<Vector3> GetPathPoints()
    {
        return l_pathpoints;
    }



}
