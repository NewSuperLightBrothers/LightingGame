using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombManager: LaserGunWeaponSystem
{
    [SerializeField] private float _circleLength;

    [SerializeField] MeshRenderer _bombMeshRenderer;

    protected override void SetObjectTeamColor(Color color, float emissionStrength)
    {
    }
}
