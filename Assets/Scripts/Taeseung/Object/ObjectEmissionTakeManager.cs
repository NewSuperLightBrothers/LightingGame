using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectEmissionTakeManager : MonoBehaviour
{
    [SerializeField] private EObjectColorType _team;
    [SerializeField] private Material _choiceOutLineMaterial;
    [SerializeField] private LongDistance_LaserGun _laserGunManager;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private int _lightMaxAmount;

    private int _lightCurrentAmount = 0;

    //input 처리 관련 변수들, 나중에 기능 통합하면 사라질 변수들임
    private Touch _touch;
    private float _duration = 0.5f;
    private float _startTime = 0;
    private float _endTime = 0;
    private float _readyTime = 0;
    private float _readyEndTime = 0;
    private bool _isTouch = false;
    private ObjectEmissionManager _emissionManager;


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print(_lightCurrentAmount);
            GiveTheLightEnergyToGun();
        }
        checkChargingmode();

    }

    private void checkChargingmode()
    {
        //터치가 시작되었을시
        if (Input.touchCount > 0 && _isTouch == false)
        {
            _touch = Input.GetTouch(0);
            _startTime = Time.time;
            _isTouch = true;
        }
        //터치가 끝날시
        else if (Input.touchCount <= 0 && _isTouch == true)
        {
            _endTime = 0;
            _startTime = 0;
            _readyTime = Time.time;
            _isTouch = false;
            _laserGunManager.enabled = true;
            StopChargingSound();
        }

        if (_isTouch == true)
            _endTime = Time.time;

        else if (_isTouch == false)
            _readyEndTime = Time.time;


        if (_duration < _endTime - _startTime && _isTouch == true)
        {
            //일정 시간 누른 경우, 해당 위치에 흡수 가능 빛이 있는지 확인
            if (_emissionManager != null) _emissionManager.TurnOffUI();

            if (takeRaycastObject(_touch))
            {
                if (_emissionManager.takeLightEnergy(_team))
                {
                    PlayChargingSound();
                    TakeLightEnergy(1);
                    _laserGunManager.enabled = false;
                }
                //현재 색깔로 흡수 가능한 물체가 맞는지 확인
                //흡수 가능한 물체면 gauge를 채움
            }
        }
        else if (_duration * 5 < _readyEndTime - _readyTime && _isTouch == false)
        {
            if (_emissionManager != null)
            {
                _emissionManager.TurnOffUI();
                _emissionManager = null;
                _readyEndTime = 0;
                _readyTime = 0;
            }
        }
    }





    private bool takeRaycastObject(Touch touch)
    {
        _emissionManager = null;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300, LayerMask.GetMask("LightObject")))
            {
                //Debug.Log("롱 프레스한 오브젝트: " + hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent<ObjectEmissionManager>(out _emissionManager))
                {
        
                    /* 클릭 오브젝트에 대한 외곽선 표시...
                    MeshRenderer renderer;
                    if (hit.collider.TryGetComponent<MeshRenderer>(out renderer))
                    {

                    }
                    */
                    return true;
                }
                else
                    return false;
            }
            else return false;
    }

    private void PlayChargingSound()
    {
        /*
        if (audioSrc.enabled == false)
        {
            audioSrc.enabled = true;
            audioSrc.Play();
            //audioSrc.loop = true;
        }
        else
        {
            if (audioSrc.clip.length - 0.5f < audioSrc.time)
            {
                audioSrc.time = audioSrc.clip.length - 0.6f;
            }
        }*/
    }

    private void StopChargingSound()
    {
        audioSrc.Stop();
        audioSrc.loop = false;
        audioSrc.enabled = false;

    }

    private void TakeLightEnergy(int k)
    {
        if (_lightCurrentAmount<_lightMaxAmount)
            _lightCurrentAmount += k;
    }

    public void GiveTheLightEnergyToGun()
    {
        int maxGauge = _laserGunManager.getWeaponMaxGauge();
        int remainGauge = _laserGunManager.GetWeaponGauge();


        if (_lightCurrentAmount > maxGauge - remainGauge)
        {
            print("!");
            _laserGunManager.SetWeaponGauge(maxGauge - remainGauge);
            _lightCurrentAmount -= (maxGauge - remainGauge);
        }
        else if(_lightCurrentAmount <= 0)
        {
            print("no light");
        }
        else
        {
            print("2");
            _laserGunManager.SetWeaponGauge(_lightCurrentAmount);
            _lightCurrentAmount -= _lightCurrentAmount;
        }
        
       
    }
    

}
