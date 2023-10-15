﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SubWeapon_BombManager: SubWeaponManager, WeaponInterface
{
    [SerializeField] private GameObject _bombPrefab;                

    [SerializeField] private float _bombPrefabThrowPower = 1;         // 투척력
    [SerializeField] private int _bombPrefabPointsCount = 50;         // 궤적을 나타내는 점의 개수
    [SerializeField] private float _bombPrefabPointsIntervalTime = 0; // 궤적을 나타날때 점과 점의 사이 시간
    [SerializeField] private LineRenderer _bombPrefabLineRenderer;    // 궤적을 나타낼 실제 라인 렌더러

    [SerializeField] private Transform _bombPrefabFirePoint;
    private Vector3 _bomblaunchVelocity;
    private Vector3 _bombPrefabFirePointPosition;

    private new void Start()
    {
        base.Start();
        _bombPrefabLineRenderer.positionCount = _bombPrefabPointsCount;
        _bombPrefabFirePointPosition = _bombPrefabFirePoint.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _weaponCount > 0)
        {
            StartAttack();

        }
    }


    private void FixedUpdate()
    {
        if (_bombPrefabFirePointPosition != _bombPrefabFirePoint.position || _bomblaunchVelocity != _bombPrefabFirePoint.forward * _bombPrefabThrowPower)
        {
            _bomblaunchVelocity = _bombPrefabFirePoint.forward * _bombPrefabThrowPower;
            CheckAttackRange();
        }

    }


    private Vector3 BombCalculateTrajectoryPoint(float time)
    {
        // 시간 t에 따른 예상 위치를 계산하는 로직을 구현
        Vector3 position = _bombPrefabFirePoint.position + _bomblaunchVelocity * time;
        position += 0.5f * Physics.gravity * time * time; // 중력 적용
        return position;
    }

    private void OnEnable() =>    _bombPrefabLineRenderer.enabled = true;
    private void OnDisable()=>    _bombPrefabLineRenderer.enabled = false;

    public void StartAttack()
    {
        SubWeapon_Bomb tempBomb = Instantiate(_bombPrefab).GetComponent<SubWeapon_Bomb>();
        l_weaponMeshRenderer.Add(tempBomb.GetBombMeshRenderer());
        ColorSetting(_weaponColor, _weaponEmissionStrength);
        l_weaponMeshRenderer.RemoveAt(0);

        tempBomb.BombInitializeSetting(_weaponRemainTime, _bombPrefabFirePoint.position, _bomblaunchVelocity);
        tempBomb.SetBombDamage(_weaponDamage);
        _weaponCount -= 1;
    }

    public void CheckAttackRange()
    {
        // 궤적 시뮬레이션 및 라인 렌더러에 궤적 좌표 추가
        for (int i = 0; i < _bombPrefabPointsCount; i++)
        {
            float t = i / (float)_bombPrefabPointsCount * _bombPrefabPointsIntervalTime;
            Vector3 position = BombCalculateTrajectoryPoint(t);
            _bombPrefabLineRenderer.SetPosition(i, position);
        }
    }

    public void SetWeaponGauge(float newval)
    {
        //not need
    }

    public float GetWeaponGauge()
    {
        return 0;//not need
    } 

}