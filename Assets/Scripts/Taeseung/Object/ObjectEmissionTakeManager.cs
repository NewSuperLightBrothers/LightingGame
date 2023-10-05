using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectEmissionTakeManager : MonoBehaviour
{
    [SerializeField] private EObjectColorType _team;
    [SerializeField] private Material _choiceOutLineMaterial;

    private Touch _touch;
    private float _duration = 0.5f;
    private float _startTime = 0;
    private float _endTime = 0;
    private float _readyTime = 0;
    private float _readyEndTime = 0;
    private bool _isTouch = false;

    private ObjectEmissionManager _emissionManager;
    [SerializeField] private LongDistance_LaserGun _laserGunManager2;


    private void Update()
    {
        if (Input.touchCount > 0 && _isTouch == false)
        {
            _touch = Input.GetTouch(0);
            _startTime = Time.time;
            _isTouch = true;
        }
        else if (Input.touchCount <= 0 && _isTouch == true)
        {
            _endTime = 0;
            _startTime = 0;
            _readyTime = Time.time;
            _isTouch = false;
        }

        if (_isTouch == true)
            _endTime = Time.time;
        else if (_isTouch == false)
            _readyEndTime = Time.time;


        if (_duration < _endTime - _startTime && _isTouch == true){//일정 시간 누른 경우, 해당 위치에 흡수 가능 빛이 있는지 확인
            if (_emissionManager != null) _emissionManager.TurnOffUI();

            if (takeRaycastObject(_touch)) { 
                if (_emissionManager.takeLightEnergy(_team)) 
                    //_laserGunManager.SetGauge(_emissionManager.getWeight());
                    _laserGunManager2.SetWeaponGauge(_emissionManager.getWeight());
                //현재 색깔로 흡수 가능한 물체가 맞는지 확인
                 //흡수 가능한 물체면 gauge를 채움
            }
        }
        else if(_duration * 5 < _readyEndTime - _readyTime && _isTouch == false) {
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

}
