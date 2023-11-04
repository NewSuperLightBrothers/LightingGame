using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponManager : WeaponSystem
{
    [Space]
    [Header("WEAPON SFX INFO")]
    [SerializeField] protected List<MeshRenderer> l_weaponMeshRenderer;
    [SerializeField] protected List<AudioSource> l_weaponSound;
    [SerializeField] protected List<LaserParticleSystem> l_weaponParticleSystem;
    [SerializeField] protected SerializeDictionary<string, LineRenderer> SD_weaponLineRenderer;

    [Space]
    [Header("WEAPON SUB DETAIL")]
    [SerializeField] protected short _weaponCount;
    [SerializeField] protected float _weaponRemainTime;


    protected void Start()
    {
        _weaponColor = getTeamColor(_teamColor);
        SDictionaryInitialize();
    }


    protected override void ColorSetting(Color color, float emissionStrength)
    {
        foreach (MeshRenderer i in l_weaponMeshRenderer)
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

    private void SDictionaryInitialize()
    {
        SD_weaponLineRenderer.InitializeList();
    }

}
