using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
public class AnimateController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] KinematicCharacterMotor kinematicCharacterMotor;
    static float velocity;
    private void Start()
    {
    }
    private void Update()
    {
        Run();
    }
    private void Run()
    {
        velocity = kinematicCharacterMotor.Velocity.magnitude;
        animator.SetFloat("Velocity", velocity);
    }
    public void Atk()
    {
        animator.SetTrigger("Attack");
    }
}
