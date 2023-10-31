using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;

public class NCharacter : NetworkBehaviour
{
    public Joystick joystick;

    private void Update()
    {
        if (joystick != null)
        {
            if (IsOwner)
            {
                if (joystick.Horizontal > 0.5f)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * 5f);
                }
                else if (joystick.Horizontal < -0.5f)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * 5f);
                }

                if (joystick.Vertical > 0.5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * 5f);
                }
                else if (joystick.Vertical < -0.5f)
                {
                    transform.Translate(Vector3.back * Time.deltaTime * 5f);
                }
            }
        }
            
    }
}
