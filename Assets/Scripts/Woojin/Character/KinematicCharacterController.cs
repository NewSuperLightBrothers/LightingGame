using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicCharacterController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private float moveSpeed;

    private Rigidbody _rigidbody;
    private Vector3 _velocityOS;
    private Vector3 _velocityTS;
    private Vector3 _velocityWS;
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        _velocityOS = Vector2XZ(_inputManager.InputData.velocityIS) * moveSpeed;
        _velocityTS = _velocityOS;
        _velocityWS = _velocityTS;
        _rigidbody.MovePosition(_rigidbody.position + _velocityWS * Time.fixedDeltaTime);
    }

    private Vector3 Vector2XZ(Vector2 value) {
        return value.x * Vector3.right + value.y * Vector3.forward;
    }
}
