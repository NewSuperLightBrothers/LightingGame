using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponManager : WeaponSystem
{
    [Space]
    [SerializeField] protected int _weaponGauge;
    [SerializeField] protected int _weaponAttackConsumeGauge;
    [SerializeField] protected float _weaponDelayTime;
    [SerializeField] protected GameObject _weaponObject;
    [SerializeField] protected ObjectEmissionTakeManager _weaponObjectTakingManager;
    protected int _weaponRemainGauge;

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
        materialColor = ObjectData.d_objectColor[colorType];
        return materialColor;
    }

    private void SDictionaryInitialize()
    {
        SD_weaponAttackAnimation.InitializeList();
        SD_weaponLineRenderer.InitializeList();
    }

    public int getWeaponMaxGauge() => _weaponGauge;

}
