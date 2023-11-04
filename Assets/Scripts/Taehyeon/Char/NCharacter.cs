using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

public class NCharacter : NetworkBehaviour
{
    public Joystick joystick;
    public Transform cameraFollowPoint;

    // UI
    public Button jumpBtn;

    private Rigidbody rb;

    // stat
    public float jumpForce = 10f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void ConnectUI()
    {
        jumpBtn.onClick.AddListener(() =>
        {
            Logger.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        });
    }

    
    private void Update()
    {
        if (joystick == null || !IsOwner) return;

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

    [ServerRpc]
    public void TestServerRPC(ServerRpcParams rpcParams)
    {
        Logger.Log(OwnerClientId + " / " + rpcParams.Receive.SenderClientId);
    }
}
