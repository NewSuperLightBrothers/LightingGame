using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MoveState {
    public string name;
    public float moveSpeed;
}

[System.Serializable]
public class CharacterStat{
    public enum TeamColor{
        Red, Green, Blue
    }
    public TeamColor teamColor;

    public List<MoveState> l_MoveState;
    public readonly float maxHealthPoints = 100f;
    public float healthPoints;
    public float moveSpeed;
    public float jumpSpeed;
}

public class DynamicCharacterState : MonoBehaviour
{
    private GameObject _mainCharacter;
    private DynamicCharacterController _dynamicCharacterController;

    public CharacterStat characterStat;
    public UnityEvent OnCharacterDestroy;

    private void Awake() {
        _mainCharacter = this.gameObject;
        _dynamicCharacterController = _mainCharacter.GetComponent<DynamicCharacterController>();
        characterStat.healthPoints = characterStat.maxHealthPoints;
    }

    private void Update() {
        UpdateMoveSpeed(characterStat.moveSpeed);
    }

    private void UpdateMoveSpeed(float moveSpeed) {
        _dynamicCharacterController.maxGroundVelocity = characterStat.moveSpeed;
    }

    public void healthAdd(float value) {
        characterStat.healthPoints += value;
        characterStat.healthPoints = Mathf.Clamp(characterStat.healthPoints, 0f, characterStat.maxHealthPoints);
        if (characterStat.healthPoints == 0f) {
            OnCharacterDestroy.Invoke();
        }
    }

    
}
