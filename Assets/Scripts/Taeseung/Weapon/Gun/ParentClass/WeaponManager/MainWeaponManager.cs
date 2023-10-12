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
    [SerializeField] protected List<LaserParticleSystem> l_weaponParticleSystem;
    [SerializeField] protected SerializeDictionary<string, Animator> SD_weaponAttackAnimation;
    [SerializeField] protected SerializeDictionary<string, LineRenderer> SD_weaponLineRenderer;

    protected void Start()
    {
        _weaponColor = getTeamColor(_teamColor);
        ColorSetting(_weaponColor, _weaponEmissionStrength);
        SDictionaryInitialize();
    }

    protected override void ColorSetting(Color color, float emissionStrength)
    {
        foreach(MeshRenderer i in l_weaponMeshRenderer)
        {
            i.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
        }

        
        foreach (LineRenderer j in SD_weaponLineRenderer.Getvalues())
        {
            j.material.SetColor("_EmissionColor", color * Mathf.Pow(2, emissionStrength));
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

    private void SDictionaryInitialize()
    {
        SD_weaponAttackAnimation.InitializeList();
        SD_weaponLineRenderer.InitializeList();
    }

}
