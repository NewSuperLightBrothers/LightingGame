using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;

[System.Serializable]
public class CharacterData{
    public float healthPoints;
    public float moveSpeed;
    public float jumpSpeed;
}

public class CharacterState : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;
    private ExampleCharacterController exampleCharacterController;
    private KinematicCharacterMotor kinematicCharacterMotor;

    private void Awake() {
        exampleCharacterController = _mainCharacter.GetComponent<ExampleCharacterController>();
        kinematicCharacterMotor = exampleCharacterController.Motor;
    }

    
}
