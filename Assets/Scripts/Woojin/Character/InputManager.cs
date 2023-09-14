using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


[System.Serializable]
public class UserInputData {
    public Vector2 velocityIS;
    public Vector2 swipeIS;
    public bool isJump;
    public bool isRun;
    public bool isFire;
    public Vector3 characterForwardWS;
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private UserInputData _inputData;
    public UserInputData InputData => _inputData;
    private UserInputAssets _userInputAssets;
    [SerializeField] private RectTransform _joystickForeground;
    private Rect _rect;
    private Image _image;

    private Vector2 mouseDelta = Vector2.zero;

    public bool b, oldB, value;
    private Vector2 touchBefore;

    [System.Serializable]
    private enum FingerData
    {
        None, ThumbStick, Screen, Fire
    }
    [SerializeField] private FingerData[] _fingerData = new FingerData[10];

    private void Awake() {
        _userInputAssets = new();

        _userInputAssets.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.TouchSimulation.Enable();

        _userInputAssets.Locomotion.Move.started -= OnMove;
        _userInputAssets.Locomotion.Move.performed -= OnMove;
        _userInputAssets.Locomotion.Move.canceled -= OnMoveCancel;

        _userInputAssets.Locomotion.View.started -= OnView;
        _userInputAssets.Locomotion.View.performed -= OnView;
        _userInputAssets.Locomotion.View.canceled -= OnView;

        _userInputAssets.Locomotion.Jump.started -= OnJump;
        _userInputAssets.Locomotion.Jump.performed -= OnJump;
        _userInputAssets.Locomotion.Jump.canceled -= OnJump;

        _userInputAssets.Interaction.Fire.started -= OnFire;
        _userInputAssets.Interaction.Fire.performed -= OnFire;
        _userInputAssets.Interaction.Fire.canceled -= OnFire;

        _userInputAssets.Locomotion.Move.started += OnMove;
        _userInputAssets.Locomotion.Move.performed += OnMove;
        _userInputAssets.Locomotion.Move.canceled += OnMoveCancel;

        _userInputAssets.Locomotion.View.started += OnView;
        _userInputAssets.Locomotion.View.performed += OnView;
        _userInputAssets.Locomotion.View.canceled += OnView;

        _userInputAssets.Locomotion.Jump.started += OnJump;
        _userInputAssets.Locomotion.Jump.performed += OnJump;
        _userInputAssets.Locomotion.Jump.canceled += OnJump;

        _userInputAssets.Interaction.Fire.started += OnFire;
        _userInputAssets.Interaction.Fire.performed += OnFire;
        _userInputAssets.Interaction.Fire.canceled += OnFire;

        _userInputAssets.TouchFallback.LeftButton.started += OnTouchFallbackModifier;
        _userInputAssets.TouchFallback.LeftButton.performed += OnTouchFallbackModifier;
        _userInputAssets.TouchFallback.LeftButton.canceled += OnTouchFallbackModifier;

        _userInputAssets.TouchFallback.Position.started += OnTouchFallback;
        _userInputAssets.TouchFallback.Position.performed += OnTouchFallback;
        _userInputAssets.TouchFallback.Position.canceled += OnTouchFallback;
    }
    
    private void OnDisable() {
        _userInputAssets.Locomotion.Move.started -= OnMove;
        _userInputAssets.Locomotion.Move.performed -= OnMove;
        _userInputAssets.Locomotion.Move.canceled -= OnMoveCancel;

        _userInputAssets.Locomotion.View.started -= OnView;
        _userInputAssets.Locomotion.View.performed -= OnView;
        _userInputAssets.Locomotion.View.canceled -= OnView;

        _userInputAssets.Locomotion.Jump.started -= OnJump;
        _userInputAssets.Locomotion.Jump.performed -= OnJump;
        _userInputAssets.Locomotion.Jump.canceled -= OnJump;

        _userInputAssets.Interaction.Fire.started -= OnFire;
        _userInputAssets.Interaction.Fire.performed -= OnFire;
        _userInputAssets.Interaction.Fire.canceled -= OnFire;

        _userInputAssets.TouchFallback.Position.started -= OnTouchFallback;
        _userInputAssets.TouchFallback.Position.performed -= OnTouchFallback;
        _userInputAssets.TouchFallback.Position.canceled -= OnTouchFallback;
        
        _userInputAssets.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }
    private void OnMove(InputAction.CallbackContext c) {
        _inputData.velocityIS = c.ReadValue<Vector2>();
    }
    private void OnMoveCancel(InputAction.CallbackContext c) {
        _inputData.velocityIS = Vector2.zero;
    }
    private void OnView(InputAction.CallbackContext c) {
        
    }
    private void OnJump(InputAction.CallbackContext c) {
        _inputData.isJump = c.ReadValue<float>() == 1f;
    }
    private void OnFire(InputAction.CallbackContext c) {
        _inputData.isFire = c.ReadValue<float>() == 1f;
    }
    private void OnTouchFallbackModifier(InputAction.CallbackContext c) {
        b = c.ReadValue<float>() == 1;
    }
    private void OnTouchFallback(InputAction.CallbackContext c) {
        mouseDelta = b? c.ReadValue<Vector2>() : Vector2.zero;
    }

    private void Start() {
        _rect = new Rect(_joystickForeground.rect);
        _image = _joystickForeground.gameObject.GetComponent<Image>();
        _rect.position = _joystickForeground.parent.GetComponent<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
    }

    private void Update() {
        InputData.swipeIS = GetTouchDeltaEnhanced();

        
    }

    private void FixedUpdate() {
        Debug.Log(_userInputAssets.TouchFallback.Position.ReadValue<Vector2>());
        if ((!oldB && b) && !value && !_rect.Contains(_userInputAssets.TouchFallback.Position.ReadValue<Vector2>())) value = true;
        if (!oldB && !b) value = false;

        
        if (value && touchBefore != Vector2.zero && mouseDelta != Vector2.zero) {
            Vector2 touchNow = mouseDelta;
            InputData.swipeIS += touchNow - touchBefore;
            
        }
        

        touchBefore = mouseDelta;
        oldB = b;
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

    private Vector2 GetTouchDeltaInput() {
        if (Input.touchCount <= 0) return Vector2.zero;

        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == UnityEngine.TouchPhase.Moved && !_rect.Contains(touch.position)) {
                return Input.GetTouch(i).deltaPosition;
            }
        }
        return Vector2.zero;
    }

}