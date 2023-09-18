using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무기 투사체 대한 전반적인 기능 정의를 하는 부분
public abstract class LaserGunManager : LaserGunWeaponSystem
{
    // Start is called before the first frame update

    [Header("레이저 정보")]
    [SerializeField]
    protected Laserinfo _laserInfo;

    protected Vector3 _startPosition;
    protected Vector3 _bulletForwardVector;
    protected Vector3 _rayHitPos;
    protected float _rayHitPosdistance = -1;
    protected Vector3 _rayOppositeNormal;
    protected Ray _ray = new();


    protected abstract void LaserBulletDestroy();
    protected abstract void LaserBulletFire();
    protected abstract void LaserBulletReflection();
    protected abstract void LaserBulletToPlayer(Collider other);


    private void Start()
    {
        VectorInitialize(transform.position, transform.forward);
        MakeMirrorRayhitInfo(_ray, 500);
        SetObjectTeamColor(_materialcolor, _emissionStrength);
    }


    protected void VectorInitialize(Vector3 newstartPosition, Vector3 forwardvector)
    {
        _startPosition = newstartPosition;
        _bulletForwardVector = forwardvector;

        _ray.direction = _bulletForwardVector;
        _ray.origin = _startPosition;
    }


    protected void MakeMirrorRayhitInfo(Ray ray, float distance)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, LayerMask.GetMask("Mirror"));
        if (hits.Length > 0)
        {
            _rayHitPos = hits[0].point;
            _rayHitPosdistance = Vector3.Distance(_startPosition, _rayHitPos);
            _rayOppositeNormal = hits[0].normal;

            Debug.DrawLine(_startPosition, _rayHitPos , Color.red, 10);
        }
        else
        {
            _rayHitPosdistance = -1;
        }
    }

    protected override void SetObjectTeamColor(Color color, float emissionstrength)
    {
        _laserInfo.bulletlinerenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionstrength));
    }
}
