using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무기 투사체 대한 전반적인 기능 정의를 하는 부분
public abstract class LaserGunManager : LaserGunWeaponSystem
{
    [Header("레이저 정보")]
    [SerializeField] protected LaserInfo _laserInfo;

    protected Vector3 _startPosition;
    protected Vector3 _bulletForwardVector;
    protected Vector3 _rayHitPos;
    protected float _rayHitPosDistance = -1;
    protected Vector3 _rayOppositeNormal;
    protected Ray _ray = new();


    protected abstract void LaserBulletDestroy();
    protected abstract void LaserBulletFire();
    protected abstract void LaserBulletReflection();
    protected abstract void LaserBulletToPlayer(Collider other);


    private void Start()
    {
        VectorInitialize(transform.position, transform.forward);
        MakeMirrorRayHitInfo(_ray, 500);
        SetObjectTeamColor(_materialColor, _emissionStrength);
    }


    protected void VectorInitialize(Vector3 newStartPosition, Vector3 forwardVector)
    {
        _startPosition = newStartPosition;
        _bulletForwardVector = forwardVector;

        _ray.direction = _bulletForwardVector;
        _ray.origin = _startPosition;
    }


    protected void MakeMirrorRayHitInfo(Ray ray, float distance)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, LayerMask.GetMask("Mirror"));
        if (hits.Length > 0)
        {
            _rayHitPos = hits[0].point;
            _rayHitPosDistance = Vector3.Distance(_startPosition, _rayHitPos);
            _rayOppositeNormal = hits[0].normal;

            Debug.DrawLine(_startPosition, _rayHitPos , Color.red, 10);
        }
        else
        {
            _rayHitPosDistance = -1;
        }
    }

    protected override void SetObjectTeamColor(Color color, float emissionStrength)
    {
        _laserInfo.bulletLineRenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
    }
}
