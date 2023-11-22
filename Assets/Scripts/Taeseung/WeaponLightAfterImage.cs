using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLightAfterImage : MonoBehaviour
{
    private EObjectColorType _colorType;
    private float _remainTime = 5;
    private float _currentTime = 0;
    private float _speedBuff;


    public float GetSpeed() => _speedBuff;

    public void SetColorType(EObjectColorType colorType) {
        _colorType = colorType;
    }

    public EObjectColorType GetColorType() => _colorType;

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_remainTime <= _currentTime)
            Destroy(this.gameObject);
    }

}
