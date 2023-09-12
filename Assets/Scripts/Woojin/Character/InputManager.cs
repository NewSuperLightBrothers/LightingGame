using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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
    private Vector2 touchBefore;

    private Vector2 mouseDelta = Vector2.zero;

    private void Awake() {
        _userInputAssets = new();

        _userInputAssets.Enable();

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

        //_userInputAssets.PrimaryTouch.Delta.started += OnTouchStart;
        _userInputAssets.PrimaryTouch.Delta.performed += OnTouch;
        _userInputAssets.PrimaryTouch.Delta.canceled += OnTouch;
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

        //_userInputAssets.PrimaryTouch.Delta.started -= OnTouchStart;
        _userInputAssets.PrimaryTouch.Delta.performed -= OnTouch;
        _userInputAssets.PrimaryTouch.Delta.canceled -= OnTouch;
        
        _userInputAssets.Disable();
    }
    private void OnMove(InputAction.CallbackContext c) {
        _inputData.velocityIS = c.ReadValue<Vector2>();
    }
    private void OnMoveCancel(InputAction.CallbackContext c) {
        _inputData.velocityIS = Vector2.zero;
    }
    private void OnView(InputAction.CallbackContext c) {
        mouseDelta = c.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext c) {
        _inputData.isJump = c.ReadValue<float>() == 1f;
    }
    private void OnFire(InputAction.CallbackContext c) {
        _inputData.isFire = c.ReadValue<float>() == 1f;
    }

    private void OnTouchStart(InputAction.CallbackContext c) {
        Vector2 touchPos = c.ReadValue<Vector2>();
        _inputData.swipeIS = Vector2.zero;
        touchBefore = touchPos;
    }
    private void OnTouch(InputAction.CallbackContext c) {
        Vector2 touchPos = c.ReadValue<Vector2>();
        _inputData.swipeIS = touchPos - touchBefore;
        touchBefore = touchPos;
    }
    private void OnTouchCancel(InputAction.CallbackContext c) {
        Vector2 touchPos = c.ReadValue<Vector2>();
        _inputData.swipeIS = Vector2.zero;
        touchBefore = touchPos;
    }

    private void Start() {
        _rect = new Rect(_joystickForeground.rect);
        _image = _joystickForeground.gameObject.GetComponent<Image>();
        _rect.position = _joystickForeground.parent.GetComponent<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
    }

    private void Update() {
        Debug.Log(Touchscreen.current.primaryTouch.delta.value);
        //Vector2 a = GetTouchDelta();
        //_inputData.swipeIS = a * 2f;
    }

    private Vector2 GetTouchDelta()
    {
        // Get the first touch that meets the conditions
        return Touchscreen.current.primaryTouch.delta.value;
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
//!_rect.Contains(touch.startPosition.value)