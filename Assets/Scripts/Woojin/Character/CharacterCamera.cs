using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    public Transform cameraTarget;
    [SerializeField] private Transform _mainCamera;
    [SerializeField] private Transform _aimRay;
    [SerializeField] private Transform _cameraOffset;
    [SerializeField] private Vector3 _cameraOffsetValue;
    [SerializeField] private float _cameraRadius;
    private float _cameraDistanceWS;

    private float cameraForwardVelocityOS = 0f;
    
    public void LateUpdate() {
        transform.position = cameraTarget.position;

        if (Physics.SphereCast(transform.position + transform.right * _cameraOffsetValue.x, _cameraRadius, transform.up * _cameraOffsetValue.y, out RaycastHit _hit, _cameraOffsetValue.y) 
        && Vector3.Dot(_hit.normal, _mainCamera.transform.forward) > 0f && _hit.transform.tag != "Player") {
            _cameraOffset.localPosition = new Vector3(_cameraOffsetValue.x, Vector3.Distance(_hit.point + _cameraRadius * _hit.normal, transform.position + transform.right * _cameraOffsetValue.x));
            _mainCamera.localPosition = Vector3.zero;
        } else {
            _cameraOffset.localPosition = _cameraOffsetValue;
            if (Physics.SphereCast(_aimRay.position, _cameraRadius, -_aimRay.forward, out RaycastHit hit, 5f) && hit.transform.tag != "Player") {
                _cameraDistanceWS = Vector3.Distance(_aimRay.position, hit.point + _cameraRadius * hit.normal);
            } else {
                _cameraDistanceWS = 5f;
            }
            float smoothedDistanceWS = Mathf.SmoothDamp(-_mainCamera.localPosition.z, _cameraDistanceWS, ref cameraForwardVelocityOS, 0.02f);
            _mainCamera.localPosition = smoothedDistanceWS * Vector3.back;
        }

        
    }
}
