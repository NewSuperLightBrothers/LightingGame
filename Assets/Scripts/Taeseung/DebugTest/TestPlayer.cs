using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : Singleton<TestPlayer>
{
    public float testHP;
    public GameObject Gun;
    public SubWeapon_BombManager bomb;
    public Animator animation;
    public KinematicCharacterMotor motor;
    public ExampleCharacterController controller;

    public int bombmode = 0;

    public void CreateBomb()
    {
        if (bombmode == 0)
        {
            Gun.SetActive(false);
            bomb.enabled = true;
            bombmode = 1;
        }
        else if(bombmode == 1)
        {
            Gun.SetActive(true);
            bomb.enabled = false;
            bombmode = 0;
        }
    }

    private void Update()
    {
        animation.SetFloat("Spd", motor.Velocity.magnitude/4);
        if (motor.Velocity.magnitude/4 > 1)
        {
            //print(animation.GetCurrentAnimatorStateInfo(0).shortNameHash);
            animation.speed = motor.Velocity.magnitude/4;
        }
        else
        {
            animation.speed = 1;
        }

        if (controller.JumpConsumed)
        {
            animation.SetBool("Jump", true);
        }
        else
        {
            animation.SetBool("Jump", false);
        }


    }



}
