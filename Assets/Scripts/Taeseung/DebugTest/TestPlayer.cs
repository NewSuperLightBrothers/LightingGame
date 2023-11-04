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
    public Animator playeranimation;
    public KinematicCharacterMotor motor;
    public ExampleCharacterController controller;
    public AudioSource source;

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
        playeranimation.SetFloat("Spd", motor.Velocity.magnitude/4);
        if (motor.Velocity.magnitude/4 > 1)
        {
            //print(playeranimation.GetCurrentAnimatorStateInfo(0).shortNameHash);
            playeranimation.speed = motor.Velocity.magnitude/4;
            
            source.enabled = true;

        }
        else
        {
            playeranimation.speed = 1;
            source.enabled = false;
        }

        if (controller.JumpConsumed)
        {
            playeranimation.SetBool("Jump", true);
        }
        else
        {
            playeranimation.SetBool("Jump", false);
        }


    }



}
