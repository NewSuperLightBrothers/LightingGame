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

    private Vector2 mouseDelta = Vector2.zero;

    private void Awake() {
        _userInputAssets = new();

        _userInputAssets.Enable();
        _userInputAssets.Touchscreen.Touchscreen.Enable();

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
    }
    private void OnEnable() {
        
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
        
        _userInputAssets.Disable();
        _userInputAssets.Touchscreen.Touchscreen.Disable();
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

    private void Start() {
        _rect = new Rect(_joystickForeground.rect);
        _image = _joystickForeground.gameObject.GetComponent<Image>();
        _rect.position = _joystickForeground.parent.GetComponent<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
    }

    private void Update() {
        _inputData.swipeIS = GetTouchDelta();
        Debug.Log(_inputData.swipeIS);
    }

    private Vector2 GetTouchDelta() {
        if (Touchscreen.current == null) return Vector2.zero;
        if (Touchscreen.current.touches.Count <= 0) return Vector2.zero;

        /*
        return Touchscreen.current.touches
            .Where(v => !EventSystem.current.IsPointerOverGameObject(v.touchId.ReadValue()) && v.isInProgress && !_rect.Contains(v.startPosition.value))
            .Select(v => v.delta.value).FirstOrDefault();
        */
        foreach (var v in Touchscreen.current.touches) {
            if (!EventSystem.current.IsPointerOverGameObject(v.touchId.ReadValue()) && v.isInProgress && !_rect.Contains(v.startPosition.value)) {
                return v.delta.value;
            }
        }
        return Vector2.zero;
    }

    private Vector2 GetTouchDeltaInput() {
        if (Input.touchCount <= 0) return Vector2.zero;

        if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Moved) {
            return Input.GetTouch(0).deltaPosition;
        }
        return Vector2.zero;
    }

}
