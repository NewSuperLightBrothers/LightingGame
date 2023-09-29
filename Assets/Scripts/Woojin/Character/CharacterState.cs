using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CharacterData{
    public enum TeamColor{
        Red, Green, Blue
    }
    public TeamColor teamColor;
    public readonly float maxHealthPoints = 100f;
    public float healthPoints;
    public float moveSpeed;
    public float jumpSpeed;
}

public class CharacterState : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;
    private ExampleCharacterController _characterController;
    private KinematicCharacterMotor _kinematicCharacterMotor;

    public CharacterData characterData;
    public UnityEvent OnCharacterDestroy;

    private void Awake() {
        _characterController = _mainCharacter.GetComponent<ExampleCharacterController>();
        _kinematicCharacterMotor = _characterController.Motor;
        characterData.healthPoints = characterData.maxHealthPoints;
    }

    private void Update() {
        UpdateMoveSpeed(characterData.moveSpeed);
    }

    private void UpdateMoveSpeed(float moveSpeed) {
        _characterController.MaxStableMoveSpeed = characterData.moveSpeed;
        _characterController.MaxAirMoveSpeed = characterData.moveSpeed;
    }

    public void healthAdd(float value) {
        characterData.healthPoints += value;
        characterData.healthPoints = Mathf.Clamp(characterData.healthPoints, 0f, characterData.maxHealthPoints);
        if (characterData.healthPoints == 0f) {
            OnCharacterDestroy.Invoke();
        }
    }

    
}
