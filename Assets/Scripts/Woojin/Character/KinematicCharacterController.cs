using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicCharacterController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Transform _orientation;

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float moveSpeed;


    private Rigidbody _rigidbody;
    private Vector3 _velocityOS;
    private Vector3 _velocityTS;
    private Vector3 _velocityWS;
    
    private Vector2 rotationOS = Vector2.zero;
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        RotateCamera();

        _velocityOS = Vector2XZ(_inputManager.InputData.velocityIS) * moveSpeed;
        _velocityTS = _orientation.rotation * _velocityOS;
        _velocityWS = _velocityTS;
        _rigidbody.MovePosition(_rigidbody.position + _velocityWS * Time.fixedDeltaTime);
    }

    private void RotateCamera() {
        Vector2 rotationCS = _inputManager.InputData.swipeIS * 0.1f;
        rotationOS += rotationCS;
        rotationOS.x = Mathf.Repeat(rotationOS.x, 360f);
        rotationOS.y = Mathf.Clamp(rotationOS.y, -90f, 90f);
        Quaternion rotationWS = Quaternion.Euler(-rotationOS.y, rotationOS.x, 0);

        _cameraTarget.rotation = rotationWS;
    }
    private Vector3 Vector2XZ(Vector2 value) {
        return value.x * Vector3.right + value.y * Vector3.forward;
    }
}
