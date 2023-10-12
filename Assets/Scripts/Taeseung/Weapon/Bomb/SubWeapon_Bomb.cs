using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SubWeapon_Bomb: MonoBehaviour
{
    [SerializeField] private GameObject _bombExplosionObject;   //bomb 폭발 시 생성될 생성 이펙트 오브젝트
    [SerializeField] private MeshRenderer _bombMeshRenderer;    //bomb 색깔 입힐 메쉬 랜더러
    
    private float _bombDamage;                 
    private float _bombExplosionTime;
    private float _realTime = 0;

    private void FixedUpdate()
    {
        _realTime += Time.deltaTime;
        if (_realTime > _bombExplosionTime)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Instantiate(_bombExplosionObject).transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    public MeshRenderer GetBombMeshRenderer() => _bombMeshRenderer;

    public void BombInitializeSetting(float remainTime, Vector3 position, Vector3 launchVelocity)
    {
        _bombExplosionTime = remainTime;
        this.transform.position = position;
        this.GetComponent<Rigidbody>().AddForce(launchVelocity, ForceMode.VelocityChange);
    }

    public void SetBombDamage(float damage)
    {
        _bombDamage = damage;
    }
}
