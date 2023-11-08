using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class NCharacter : NetworkBehaviour
{
    [HideInInspector] public Joystick joystick;
    public NetworkVariable<EObjectColorType> teamColor;
    
    // UI
    [HideInInspector] public Button jumpBtn;
    [HideInInspector] public Button fireBtn;
    
    // Components
    private Animator animator;
    private CharacterController _controller;
    [HideInInspector] public GameObject mainCamera;
    
    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    // stat
    public float jumpForce = 10f;
    public bool isMoving;
    public float moveSpeed = 2.0f;
    
    // move
    public Vector2 move;
    
    // jump
    public bool jump;
    
    // look
    public Vector2 look;
    public float lookSensitivity;
    // weapon
    public LongDistance_LaserGun gun;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    
    // input
    public float _threshold = 0.02f;
    
    public float curYRot;
    
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    
    [FormerlySerializedAs("CinemachineCameraTarget")]
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public Transform cinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    #region Ground

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;


    #endregion
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }


    public void ConnectUI()
    {
        jumpBtn.onClick.AddListener(() =>
        {
            Logger.Log("Jump");
            jump = true;
        });
        
        fireBtn.onClick.AddListener(() =>
        {
            Logger.Log("Fire");
            gun.StartAttack();
        });
    }

    
    private void Update()
    {
        if (joystick == null || !IsOwner) return;

        isMoving = false;
        if (joystick.Direction != Vector2.zero)
        {
            isMoving = true;
            move.x = joystick.Horizontal;
            move.y = joystick.Vertical;
        }
        else
        {
            move = Vector2.zero;
        }
        AnimationServerRPC(isMoving);

        GroundedCheck();
        JumpAndGravity();
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    [ServerRpc]
    private void AnimationServerRPC(bool isRun, ServerRpcParams rpcParams = default)
    {
        // Logger.Log(OwnerClientId + " / " + rpcParams.Receive.SenderClientId);
        animator.SetBool("isRunning", isRun);
    }
    
    [ClientRpc]
    public void SetPosClientRPC(Vector3 pos)
    {
        Logger.Log("SetPosClientRPC called");
        transform.position = pos;
    }

    public void OnTouchLookEvent(Vector2 newLook)
    {
        look = newLook;
    }
    
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    } 
    
    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        // if (_hasAnimator)
        // {
        //     _animator.SetBool(_animIDGrounded, Grounded);
        // }
    }
    
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            // if (_hasAnimator)
            // {
            //     _animator.SetBool(_animIDJump, false);
            //     _animator.SetBool(_animIDFreeFall, false);
            // }

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                // if (_hasAnimator)
                // {
                //     _animator.SetBool(_animIDJump, true);
                // }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                // if (_hasAnimator)
                // {
                //     _animator.SetBool(_animIDFreeFall, true);
                // }
            }

            // if we are not grounded, do not jump
            jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    
    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = moveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        // float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        float inputMagnitude = move.magnitude;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        // if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        // if (_hasAnimator)
        // {
        //     _animator.SetFloat(_animIDSpeed, _animationBlend);
        //     _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        // }
    }
    
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += look.x * deltaTimeMultiplier * lookSensitivity;
            _cinemachineTargetPitch += look.y * deltaTimeMultiplier * lookSensitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
