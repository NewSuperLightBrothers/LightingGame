using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmissionTakeManager : MonoBehaviour
{
    [SerializeField]
    private ObjectColorType _team;
    [SerializeField]
    private Material _choiceooutlinematerial;

    private Touch _touch;
    private float _duration = 0.5f;
    private float _starttime = 0;
    private float _endtime = 0;
    private float _readytime = 0;
    private float _readyendtime = 0;
    private bool _isTouch = false;

    private ObjectEmissionManager emissionmanager;
    [SerializeField]
    private LaserGunShootingManager LasergunManager;
    

    void Update()
    {
        if (Input.touchCount > 0 && _isTouch == false)
        {
            _touch = Input.GetTouch(0);
            _starttime = Time.time;
            _isTouch = true;
        }
        else if (Input.touchCount <= 0 && _isTouch == true)
        {
            _endtime = 0;
            _starttime = 0;
            _readytime = Time.time;
            _isTouch = false;
        }

        if (_isTouch == true)
            _endtime = Time.time;
        else if (_isTouch == false)
            _readyendtime = Time.time;


        if (_duration < _endtime - _starttime && _isTouch == true){//일정 시간 누른 경우, 해당 위치에 흡수 가능 빛이 있는지 확인
            if (emissionmanager != null) emissionmanager.TurnOffUI();

            if (takeRaycastObject(_touch)) { 
                if (emissionmanager.takeLightEnergy(_team)) 
                    LasergunManager.SetGauge(emissionmanager.getWeight());           
                //현재 색깔로 흡수 가능한 물체가 맞는지 확인
                 //흡수 가능한 물체면 gauge를 채움
            }
        }
        else if(_duration * 5 < _readyendtime - _readytime && _isTouch == false) {
            if (emissionmanager != null)
            {
                emissionmanager.TurnOffUI();
                emissionmanager = null;
                _readyendtime = 0;
                _readytime = 0;
            }
        }


    }


    private bool takeRaycastObject(Touch touch)
    {
        emissionmanager = null;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300, LayerMask.GetMask("LightObject")))
            {
                //Debug.Log("롱 프레스한 오브젝트: " + hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent<ObjectEmissionManager>(out emissionmanager))
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
