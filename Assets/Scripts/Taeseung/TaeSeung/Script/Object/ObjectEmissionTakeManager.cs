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
            _isTouch = false;
            _endtime = 0;
            _starttime = 0;
            emissionmanager = null;
        }

        if (Input.touchCount > 0) _endtime = Time.time;
        
        if (_duration < _endtime - _starttime && emissionmanager == null)
        {
            takeRaycastObject(_touch);
        }
        else if(_duration < _endtime - _starttime && emissionmanager != null)
        {
            if (emissionmanager.takeLightEnergy(_team)) {
                LasergunManager.SetGauge(emissionmanager.getWeight());
            }

        }
    }


    private void takeRaycastObject(Touch touch)
    {

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("롱 프레스한 오브젝트: " + hit.collider.gameObject.name);
                hit.collider.TryGetComponent<ObjectEmissionManager>(out emissionmanager);

            /* 클릭 오브젝트에 대한 외곽선 표시...
                MeshRenderer renderer;
                if (hit.collider.TryGetComponent<MeshRenderer>(out renderer))
                {
                    
                }
            */

            }
    }

}
