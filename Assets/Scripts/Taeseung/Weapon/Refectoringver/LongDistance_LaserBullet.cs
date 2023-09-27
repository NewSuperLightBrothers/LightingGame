using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistance_LaserBullet : MonoBehaviour
{
    [SerializeField] private List<SoundManager> l_bulletSound;
    [SerializeField] private List<LaserParticleSystem> l_bulletParticle;
    [SerializeField] private float _emissionStrength;

    private float _bulletSpeed;
    private float _bulletDmg;
    private Color _bulletColor;
    private Vector3[] _bulletPathPoints;

    private int _pathPointIndex;
    private float _bulletDistance = 0;
    private bool isBulletDirect = false;

    void FixedUpdate()
    {
        if (_bulletPathPoints != null || _bulletPathPoints.Length != 0) LaserBulletFire();
    }

    void OnTriggerEnter(Collider other)
    {
        if (Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Mirror"))
        {
            //LaserBulletReflection();
            //print("1 : " +  transform.position + "2 : " + _bulletPathPoints[_pathPointIndex]);
            //print(isBulletDirect);

        }

        if (Mathf.Pow(2, other.transform.gameObject.layer) == LayerMask.GetMask("Player")) LaserBulletToPlayer(other);

    }


    public void SetBullet(float bulletSpeed, float bulletDmg, float bulletDistance, Color bulletColor, List<Vector3> bulletPathPoints)
    {
        _bulletSpeed = bulletSpeed;
        _bulletDmg = bulletDmg;
        _bulletDistance = bulletDistance;
        _bulletColor = bulletColor;
        _bulletPathPoints = bulletPathPoints.ToArray();
    }

    public void SetBulletStartTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

    }

    public void LaserBulletToPlayer(Collider other)
    {
        other.GetComponent<TestPlayer>().testHP -= _bulletDmg;
    }

    private void LaserBulletFire()
    {
        transform.position = Vector3.MoveTowards(transform.position, _bulletPathPoints[_pathPointIndex], _bulletSpeed);

        if (transform.position == _bulletPathPoints[_pathPointIndex])
        { 

            _pathPointIndex++;

            if (_pathPointIndex >= _bulletPathPoints.Length)
                LaserBulletDestroy();
            else
            {
                if (_pathPointIndex != 1)
                {
                    GameObject SoundCreate = Instantiate(l_bulletSound[0].gameObject);
                    SoundCreate.transform.position = transform.position;
                    SoundCreate.GetComponent<AudioSource>().Play();
                }

                l_bulletParticle[1].ParticleInstantiate(transform.position, this.transform.rotation);

                transform.forward = (_bulletPathPoints[_pathPointIndex] - _bulletPathPoints[_pathPointIndex - 1]);
            }
        }
        else isBulletDirect = true;
        
    }

    private void LaserBulletDestroy()
    {
        l_bulletParticle[0].ParticleInstantiate(this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }



    /*
    private void LaserBulletFire()
    {

    if (Mathf.Abs(Vector3.Distance(_startPosition, transform.position)) <= _laserInfo.distance) 
    transform.Translate(Vector3.forward * _bulletSpeed);


    else LaserBulletDestroy();

    if (_rayHitPosdistance >= 0)
    {    _pathPointIndex
    if(Vector3.Distance(_startPosition, transform.position) - _rayHitPosdistance > 0.1)
    {
        transform.position = _rayHitPos;
    }
    }

    //print(_bulletPathPoints.Count);
    }
    */



    /*
    private void LaserBulletReflection()
    {
        _laserInfo.distance -= Vector3.Distance(_startPosition, _rayHitPos);

        Vector3 forward = _bulletForwardVector.normalized;
        Vector3 collisionNormal = _rayOppositeNormal;
        transform.forward = Vector3.Reflect(forward, collisionNormal).normalized;

        VectorInitialize(_rayHitPos, transform.forward);
        MakeMirrorRayHitInfo(_ray, 500);
    }*/


}
