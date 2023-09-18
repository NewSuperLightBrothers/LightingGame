using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���⿡ ���� ����
public class LaserGunShootingManager : LaserGunWeaponShootingSystem
{
    [SerializeField]
    private List<MeshRenderer> l_gunMeshRenderer;
    [SerializeField]
    private LineRenderer firePath;
    [SerializeField]
    private int _reflectCount;

    private List<Vector3> l_pathPoints = new();
    private Vector3 _gunDirection;



    private new void Start()
    {
        base.Start();
        _gunDirection = _direction;
        SetObjectTeamColor(_materialcolor, _emissionStrength);
    }

    private void Update()
    {
        _direction = _guninfo.endpoint.position - _guninfo.firepoint.position;

        //�ѱ��� ������ raycast�� �ٽ� �ؾ��ϴ� ���
        if (_gunDirection != _direction)
        {
            _gunDirection = _direction;
            l_pathPoints.Clear();
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

        LaserGunBulletManager bulletManager = newbullet.GetComponent<LaserGunBulletManager>();
        bulletManager.points = l_pathPoints.ToArray();
        bulletManager.distance = _distance;

        _guninfo.gunanimation.Play(0);
        _guninfo.shootingsound.Play(0);

        _currentbulletcount -= _guninfo.usinggauge;
        SetGaugeUIBar();

        TestUI.testUI.setText(_currentbulletcount.ToString());

        _cooltimeinterval = 0;
        _isshoot = false;
    }


    protected override void SetObjectTeamColor(Color color, float emissionStrength)
    {
        //for (int i = 0; i < l_gunMeshRenderer.Count; i++) {
            l_gunMeshRenderer[1].material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
        //}

        firePath.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength-1));
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
            l_pathPoints.Add(Beforepoint);
            _ray.direction = Input;
            _ray.origin = Beforepoint;
            _hits = Physics.RaycastAll(_ray, distance, LayerMask.GetMask("Mirror"));

            //�ִ� �ݻ� Ƚ���� ������ ���
            if (_reflectCount <= l_pathPoints.Count - 2)
            {
                l_pathPoints.Add(Beforepoint + distance * Input);
                distance -= distance;
                break;
            }
            //�� �̻��� ƨ���� ���� ���
            else if (_hits.Length <= 0)
            {
                l_pathPoints.Add(Beforepoint + distance * Input);
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
                    l_pathPoints.Add(Beforepoint + distance * Input);
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

        firePath.positionCount = l_pathPoints.Count;
        firePath.SetPositions(l_pathPoints.ToArray());
    }


    private void SetGaugeUIBar()
    {
        Vector3 scale = l_gunMeshRenderer[1].transform.localScale;
        scale.z = (_currentbulletcount / _guninfo.maxgauge);
        l_gunMeshRenderer[1].transform.localScale = scale;

    }

    public void SetGauge(float newVal)
    {
        _currentbulletcount += newVal;
        SetGaugeUIBar();
        TestUI.testUI.setText(_currentbulletcount.ToString());
    }


    public List<Vector3> GetPathPoints()
    {
        return l_pathPoints;
    }



}
