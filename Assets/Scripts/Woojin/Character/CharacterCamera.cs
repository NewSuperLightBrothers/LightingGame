using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    public Transform followTransform;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform rayStart;
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private Vector3 cameraOffsetValue;
    [SerializeField] private float cameraRadius;
    public void LateUpdate() {
        transform.position = followTransform.position;
        if (Physics.SphereCast(transform.position + transform.right * cameraOffsetValue.x, cameraRadius, 
        transform.up * cameraOffsetValue.y, out RaycastHit _hit, cameraOffsetValue.y) && Vector3.Dot(_hit.normal, _camera.transform.forward) > 0f) {
            cameraOffset.localPosition = new Vector3(cameraOffsetValue.x, Vector3.Distance(_hit.point + cameraRadius * _hit.normal, transform.position + transform.right * cameraOffsetValue.x)-0.05f);
        } else {
            cameraOffset.localPosition = cameraOffsetValue;
        }
        if (Physics.SphereCast(rayStart.position, cameraRadius, -rayStart.forward, out RaycastHit hit, 5f)) {
            _camera.localPosition = (hit.point + cameraRadius * hit.normal - rayStart.position).magnitude * Vector3.back;
        } else {
            _camera.localPosition = 5f * Vector3.back;
        }
    }
}
