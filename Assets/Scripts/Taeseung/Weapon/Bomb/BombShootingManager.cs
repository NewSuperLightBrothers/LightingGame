using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombShootingManager: MonoBehaviour
{
    [SerializeField] private GameObject _bomb;
    [SerializeField] private float _simulationTime = 2f; // 시뮬레이션 시간
    [SerializeField] private float _power = 1;      // 파워
    [SerializeField] private int _pointsCount = 50; // 궤적을 나타내는 점의 개수
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _firePoint;

    public Vector3 _launchVelocity;
    private Vector3 _firePointPosition;
    private Vector3 _firePointForward;

    private void Start()
    {
        _lineRenderer.positionCount = _pointsCount;
        _firePointPosition = _firePoint.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) BulletFire();


    }


    private void FixedUpdate()
    {
        if (_firePointPosition != _firePoint.position || _launchVelocity != _firePoint.forward * _power)
        {
            _launchVelocity = _firePoint.forward * _power;
            ParabolaGhostRay();
        }

    }


    private Vector3 CalculateTrajectoryPoint(float time)
    {
        // 시간 t에 따른 예상 위치를 계산하는 로직을 구현

        
        Vector3 position = _firePoint.position + _launchVelocity * time;
        position += 0.5f * Physics.gravity * time * time; // 중력 적용
        return position;
    }


    private void ParabolaGhostRay()
    {
        // 궤적 시뮬레이션 및 라인 렌더러에 궤적 좌표 추가
        for (int i = 0; i < _pointsCount; i++)
        {
            float t = i / (float)_pointsCount * _simulationTime;
            Vector3 position = CalculateTrajectoryPoint(t);
            _lineRenderer.SetPosition(i, position);
        }
    }


    public void SetObjectTeamColor(Color color, float emissionStrength)
    {

    }

    public void BulletFire()
    {
        BombManager tempBomb = Instantiate(_bomb).GetComponent<BombManager>();
        tempBomb.transform.position = _firePoint.position;
        tempBomb.GetComponent<Rigidbody>().AddForce(_launchVelocity,ForceMode.VelocityChange);
        tempBomb.GetComponent<Rigidbody>().useGravity = true;
    }

}
