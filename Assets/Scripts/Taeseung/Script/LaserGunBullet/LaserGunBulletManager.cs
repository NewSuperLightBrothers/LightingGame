using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserGunBulletManager : LaserGunManager
{
    public AudioSource glasssound;
    public Vector3[] points;
    public float distance;
    private int pointsindex = 1;

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
        other.GetComponent<TestPlayer>().TestHP -= _laserinfo.dmg;
    }


    /*
    private void LaserBulletFire()
    {

        if (Mathf.Abs(Vector3.Distance(_startposition, transform.position)) <= _laserinfo.distance) 
        transform.Translate(Vector3.forward * _laserinfo.speed);


        else LaserBulletDestroy();

        if (_rayhitposdistance >= 0)
        {    
        if(Vector3.Distance(_startposition, transform.position) - _rayhitposdistance > 0.1)
        {
            transform.position = _rayhitpos;
        }
        }

        //print(points.Count);
    }
    */


    protected override void LaserBulletFire()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[pointsindex], _laserinfo.speed);

        if(transform.position == points[pointsindex] )
        {
            pointsindex ++;
            if (pointsindex >= points.Length) LaserBulletDestroy();
            else
            {
                GameObject soundcreate = Instantiate(glasssound.gameObject);
                soundcreate.transform.position = transform.position;
                soundcreate.GetComponent<AudioSource>().Play();
                _laserinfo.usinglaserParticle[1].particleInstantiate(transform.position, this.transform.rotation);

                transform.forward = (points[pointsindex] - points[pointsindex - 1]);
            }
        }
    }
    
    protected override void LaserBulletDestroy()
    {
        _laserinfo.usinglaserParticle[0].particleInstantiate(this.transform.position,this.transform.rotation);
        Destroy(this.gameObject);
    }


    protected override void LaserBulletReflection()
    {
        _laserinfo.distance -= Vector3.Distance(_startposition, _rayhitpos);

        Vector3 forward = _bulletforwardvector.normalized;
        Vector3 collisionnormal = _rayoppositenormal;
        transform.forward = Vector3.Reflect(forward, collisionnormal).normalized;

        VectorInitialize(_rayhitpos, transform.forward);
        MakeMirrorRayhitInfo(_ray, 500);


    }

}
