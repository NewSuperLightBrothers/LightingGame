using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Unity.Mathematics;


public class InputManager : MonoBehaviour
{
    public PlayerCharacterInputs characterInputs = new();

    private UserInputAssets _userInputAssets;

    public ExampleCharacterController character;
    //public ExampleCharacterCamera characterCamera;
    //public CharacterCamera characterCamera;
    public Transform cameraAnchor;

    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    private bool _isRunning;
    private bool _isCrouching;

    public RectTransform joystickForeground;
    private Rect _rect;

    private Vector2 _mouseDelta = Vector2.zero;
    private Vector2 _rotationOS = Vector2.zero;

    private Vector3 _lookInputWS = Vector3.zero;

    private void OnEnable() {
        _userInputAssets = new();
        _userInputAssets.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        _userInputAssets.Locomotion.Move.started -= OnMove;
        _userInputAssets.Locomotion.Move.performed -= OnMove;
        _userInputAssets.Locomotion.Move.canceled -= OnMoveCancel;

        _userInputAssets.Locomotion.Jump.started -= OnJump;
        _userInputAssets.Locomotion.Jump.performed -= OnJump;
        _userInputAssets.Locomotion.Jump.canceled -= OnJump;

        _userInputAssets.Locomotion.Run.started -= OnRun;

        _userInputAssets.Locomotion.Crouch.started -= OnCrouch;

        _userInputAssets.Interaction.Fire.started -= OnFire;
        _userInputAssets.Interaction.Fire.performed -= OnFire;
        _userInputAssets.Interaction.Fire.canceled -= OnFire;

        _userInputAssets.Locomotion.Move.started += OnMove;
        _userInputAssets.Locomotion.Move.performed += OnMove;
        _userInputAssets.Locomotion.Move.canceled += OnMoveCancel;

        _userInputAssets.Locomotion.Jump.started += OnJump;
        _userInputAssets.Locomotion.Jump.performed += OnJump;
        _userInputAssets.Locomotion.Jump.canceled += OnJump;

        _userInputAssets.Locomotion.Run.started += OnRun;

        _userInputAssets.Locomotion.Crouch.started += OnCrouch;

        _userInputAssets.Interaction.Fire.started += OnFire;
        _userInputAssets.Interaction.Fire.performed += OnFire;
        _userInputAssets.Interaction.Fire.canceled += OnFire;

        _userInputAssets.TouchFallback.Delta.started += OnTouchDelta;
        _userInputAssets.TouchFallback.Delta.performed += OnTouchDelta;
        _userInputAssets.TouchFallback.Delta.canceled += OnTouchDelta;
    }
    
    private void OnDisable() {
        _userInputAssets.Locomotion.Move.started -= OnMove;
        _userInputAssets.Locomotion.Move.performed -= OnMove;
        _userInputAssets.Locomotion.Move.canceled -= OnMoveCancel;

        _userInputAssets.Locomotion.Jump.started -= OnJump;
        _userInputAssets.Locomotion.Jump.performed -= OnJump;
        _userInputAssets.Locomotion.Jump.canceled -= OnJump;

        _userInputAssets.Locomotion.Run.started -= OnRun;

        _userInputAssets.Locomotion.Crouch.started -= OnCrouch;

        _userInputAssets.Interaction.Fire.started -= OnFire;
        _userInputAssets.Interaction.Fire.performed -= OnFire;
        _userInputAssets.Interaction.Fire.canceled -= OnFire;

        _userInputAssets.TouchFallback.Delta.started -= OnTouchDelta;
        _userInputAssets.TouchFallback.Delta.performed -= OnTouchDelta;
        _userInputAssets.TouchFallback.Delta.canceled -= OnTouchDelta;
        
        _userInputAssets.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }


    private void OnMove(InputAction.CallbackContext c) {
        Vector2 velocityIS = c.ReadValue<Vector2>();
        characterInputs.MoveAxisRight = velocityIS.x;
        characterInputs.MoveAxisForward = velocityIS.y;
    }
    private void OnMoveCancel(InputAction.CallbackContext c) {
        characterInputs.MoveAxisRight = 0f;
        characterInputs.MoveAxisForward = 0f;
    }
    private void OnJump(InputAction.CallbackContext c) {
        characterInputs.JumpDown = c.ReadValue<float>() == 1f;
    }
    private void OnRun(InputAction.CallbackContext c) {
        _isRunning = !_isRunning;
        character.MaxStableMoveSpeed = _isRunning ? runSpeed : walkSpeed;
        character.MaxAirMoveSpeed = _isRunning ? runSpeed : walkSpeed;
    }
    private void OnFire(InputAction.CallbackContext c) {
    }
    private void OnCrouch(InputAction.CallbackContext c) {
        _isCrouching = !_isCrouching;
        characterInputs.CrouchDown = _isCrouching;
        characterInputs.CrouchUp = !_isCrouching;
        character.SetInputs(ref characterInputs);
    }
    private void OnTouchDelta(InputAction.CallbackContext c) {
        _mouseDelta = c.ReadValue<Vector2>();
    }

    private void Start() {
        //cameraAnchor = GameObject.Find("CamArm(Clone)").transform; 
        _rect = new Rect(joystickForeground.rect);
        Image _image = joystickForeground.gameObject.GetComponent<Image>();
        _rect.position = joystickForeground.parent.GetComponent<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
    }

    private void LateUpdate() {
        _rotationOS = GetTouchDeltaEnhanced() + _mouseDelta;
        _rotationOS *= 0.1f;
        _lookInputWS += new Vector3(-_rotationOS.y, _rotationOS.x);

        _lookInputWS.x = Mathf.Clamp(_lookInputWS.x, -89f, 89f);
        _lookInputWS.y = Mathf.Repeat(_lookInputWS.y, 360f);

        cameraAnchor.rotation = Quaternion.Euler(_lookInputWS);
        characterInputs.CameraRotation = cameraAnchor.rotation;
        character.SetInputs(ref characterInputs);
    }

    private Vector2 GetTouchDeltaEnhanced()
    {
        foreach (var touch in EnhancedTouch.Touch.activeTouches) {
            if (!_rect.Contains(touch.startScreenPosition)) {
                return touch.delta;
            }
        }
        return Vector2.zero;
    }
}
