using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserGunBulletManager : LaserGunManager
{
    public AudioSource glassSound;
    public Vector3[] points;
    public float distance;
    private int _pointIndex = 1;

    private void FixedUpdate()
    {
        if (points != null || points.Length != 0) 
            LaserBulletFire();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Mirror")) LaserBulletReflection(); 
        if(Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Player")) LaserBulletToPlayer(other);

    }

    protected override void LaserBulletToPlayer(Collider other)
    {
        other.GetComponent<TestPlayer>().TestHP -= _laserInfo.dmg;
    }


    /*
    private void LaserBulletFire()
    {

        if (Mathf.Abs(Vector3.Distance(_startPosition, transform.position)) <= _laserInfo.distance) 
        transform.Translate(Vector3.forward * _laserInfo.speed);


        else LaserBulletDestroy();

        if (_rayHitPosdistance >= 0)
        {    _pointIndex
        if(Vector3.Distance(_startPosition, transform.position) - _rayHitPosdistance > 0.1)
        {
            transform.position = _rayHitPos;
        }
        }

        //print(points.Count);
    }
    */


    protected override void LaserBulletFire()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[_pointIndex], _laserInfo.speed);

        if(transform.position == points[_pointIndex] )
        {
            _pointIndex ++;
            if (_pointIndex >= points.Length) LaserBulletDestroy();
            else
            {
                GameObject SoundCreate = Instantiate(glassSound.gameObject);
                SoundCreate.transform.position = transform.position;
                SoundCreate.GetComponent<AudioSource>().Play();
                _laserInfo.usinglaserParticle[1].particleInstantiate(transform.position, this.transform.rotation);

                transform.forward = (points[_pointIndex] - points[_pointIndex - 1]);
            }
        }
    }
    
    protected override void LaserBulletDestroy()
    {
        _laserInfo.usinglaserParticle[0].particleInstantiate(this.transform.position,this.transform.rotation);
        Destroy(this.gameObject);
    }


    protected override void LaserBulletReflection()
    {
        _laserInfo.distance -= Vector3.Distance(_startPosition, _rayHitPos);

        Vector3 forward = _bulletForwardVector.normalized;
        Vector3 collisionNormal = _rayOppositeNormal;
        transform.forward = Vector3.Reflect(forward, collisionNormal).normalized;

        VectorInitialize(_rayHitPos, transform.forward);
        MakeMirrorRayhitInfo(_ray, 500);


    }

}
