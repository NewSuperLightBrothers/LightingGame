using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombManager: LaserGunWeaponSystem
{
    [SerializeField] private ELaserTeamType colortype;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _maxdistance = 50;
    [SerializeField] private GameObject _explosionObject;
   // private Vector3 _launchVelocity;
     private Vector3 _beforePosition;
   // private float _movedistance = 0;

    private float _time = 0;
    [SerializeField] private float _explosionTime = 3;


    private void Start()
    {
        SetObjectTeamColor(_materialColor, _emissionStrength);
        _beforePosition = this.transform.position;
    }


    private void FixedUpdate()
    {
        _time += Time.deltaTime;
        if (_time > _explosionTime)
        {
            DestroyObject();
        }

        
        
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        DestroyObject();
    }*/

    private void DestroyObject()
    {
        Instantiate(_explosionObject).transform.position = this.transform.position;
        Destroy(this.gameObject);
    }



    protected override void SetObjectTeamColor(Color color, float emissionStrength)
    {
       
    }

}
