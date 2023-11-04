using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSystem : MonoBehaviour
{
    [Header("WEAPON COMMON DEFAULT INFO")]
    [SerializeField] protected EObjectColorType _teamColor;
    [SerializeField] protected float _weaponDamage;
    [SerializeField] protected float _weaponEmissionStrength;
    protected Color _weaponColor;
    protected abstract void ColorSetting(Color color, float emissionStrength);
}
