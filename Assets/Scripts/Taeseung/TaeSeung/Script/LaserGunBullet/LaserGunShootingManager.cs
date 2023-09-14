using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���⿡ ���� ����
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

        //�ѱ��� ������ raycast�� �ٽ� �ؾ��ϴ� ���
        if (_gundirection != _direction)
        {
            _gundirection = _direction;
            l_pathpoints.Clear();
            BulletRayCast();
        }
        //��Ÿ���� ���� ��ġ�Է��� �������� ���, �߻�
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

            //�ִ� �ݻ� Ƚ���� ������ ���
            if (_reflectcount <= l_pathpoints.Count - 2)
            {
                l_pathpoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                break;
            }
            //�� �̻��� ƨ���� ���� ���
            else if (_hits.Length <= 0)
            {
                l_pathpoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                continue;
            }
            //ƨ���� �����ϴ� ���
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
