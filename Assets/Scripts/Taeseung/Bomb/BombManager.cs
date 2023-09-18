using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager: LaserGunWeaponSystem
{
    [SerializeField]
    private float CircleLength;

    [SerializeField]
    MeshRenderer Bombmeshrenderer;


    private void Start()
    {

    }


    protected override void SetObjectTeamColor(Color color, float emissionstrength)
    {
       
    }
}
