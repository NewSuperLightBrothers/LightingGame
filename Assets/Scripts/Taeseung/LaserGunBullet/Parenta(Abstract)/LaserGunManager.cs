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
    protected Laserinfo _laserinfo;

    protected Vector3 _startposition;
    protected Vector3 _bulletforwardvector;
    protected Vector3 _rayhitpos;
    protected float _rayhitposdistance = -1;
    protected Vector3 _rayoppositenormal;
    protected Ray _ray = new();


    protected abstract void LaserBulletDestroy();
    protected abstract void LaserBulletFire();
    protected abstract void LaserBulletReflection();
    protected abstract void LaserBulletToPlayer(Collider other);


    private void Start()
    {
        VectorInitialize(transform.position, transform.forward);
        MakeMirrorRayhitInfo(_ray, 500);
        SetObjectTeamColor(_materialcolor, _emissionstrength);
    }


    protected void VectorInitialize(Vector3 newstartposition, Vector3 forwardvector)
    {
        _startposition = newstartposition;
        _bulletforwardvector = forwardvector;

        _ray.direction = _bulletforwardvector;
        _ray.origin = _startposition;
    }


    protected void MakeMirrorRayhitInfo(Ray ray, float distance)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, LayerMask.GetMask("Mirror"));
        if (hits.Length > 0)
        {
            _rayhitpos = hits[0].point;
            _rayhitposdistance = Vector3.Distance(_startposition, _rayhitpos);
            _rayoppositenormal = hits[0].normal;

            Debug.DrawLine(_startposition, _rayhitpos , Color.red, 10);
        }
        else
        {
            _rayhitposdistance = -1;
        }
    }

    protected override void SetObjectTeamColor(Color color, float emissionstrength)
    {
        _laserinfo.bulletlinerenderer.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionstrength));
    }
}
