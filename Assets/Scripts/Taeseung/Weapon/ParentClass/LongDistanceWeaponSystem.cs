using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LongDistanceWeaponManager : MainWeaponManager
{
    [Space]
    [Header("LONG DISTANCE WEAPON INFO")]
    [SerializeField] protected GameObject _weaponUsingBullet;
    [SerializeField] protected GameObject _weaponAfterImage;
    [SerializeField] protected Transform _weaponShotPoint;
    [SerializeField] protected Transform _weaponShotEndPoint;
    [SerializeField] protected float _weaponBulletSpeed;
    [SerializeField] protected float _weaponDistance;
    protected Ray _weaponRay;
    protected RaycastHit _weaponRayHit;

}
