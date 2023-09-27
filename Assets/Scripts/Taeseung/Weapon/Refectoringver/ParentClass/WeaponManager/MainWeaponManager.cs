using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponManager : WeaponSystem
{
    [Space]
    [SerializeField] protected float _weaponGauge;
    [SerializeField] protected float _weaponAttackConsumeGauge;
    [SerializeField] protected float _weaponDelayTime;
    [SerializeField] protected GameObject _weaponObject;
    protected float _weaponRemainGauge;

    [Space]
    [Header("WEAPON SFX INFO")]
    [SerializeField] protected List<MeshRenderer> l_weaponMeshRenderer;
    [SerializeField] protected List<AudioSource> l_weaponSound;
    [SerializeField] protected List<Animator> l_weaponAttackAnimation;
    [SerializeField] protected List<LaserParticleSystem> l_weaponParticleSystem;

    protected void Start()
    {
        _weaponColor = getTeamColor(_teamColor);
        ColorSetting(_weaponColor, _weaponEmissionStrength);
    }

    protected override void ColorSetting(Color color, float emissionStrength)
    {
        foreach(MeshRenderer i in l_weaponMeshRenderer)
        {
            i.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
        }
    }

    private Color getTeamColor(EObjectColorType colorType)
    {
        Color materialColor;
        
        if (colorType == EObjectColorType.Red) materialColor = Color.red;
        else if (colorType == EObjectColorType.Blue) materialColor = Color.blue;
        else if (colorType == EObjectColorType.Green) materialColor = Color.green;
        else materialColor = new();

        return materialColor;
    }


}
