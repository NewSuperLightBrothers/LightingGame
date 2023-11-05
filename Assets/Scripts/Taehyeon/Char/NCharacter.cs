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

    public NetworkVariable<EObjectColorType> teamColor;
    
    // UI
    public Button jumpBtn;
    public Button fireBtn;
    
    private Rigidbody rb;
    private Animator animator;
    
    // stat
    public float jumpForce = 10f;
    public bool isMoving;
    
    // weapon
    public LongDistance_LaserGun gun;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    public void ConnectUI()
    {
        jumpBtn.onClick.AddListener(() =>
        {
            Logger.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        });
        
        fireBtn.onClick.AddListener(() =>
        {
            Logger.Log("Fire");
            gun.StartAttack();
        });
    }

    
    private void Update()
    {
        
        if (joystick == null || !IsOwner) return;

        isMoving = false;
        if (joystick.Direction != Vector2.zero)
        {
            isMoving = true;
        }
        AnimationServerRPC(isMoving);
        
        
        
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
    private void AnimationServerRPC(bool isRun, ServerRpcParams rpcParams = default)
    {
        // Logger.Log(OwnerClientId + " / " + rpcParams.Receive.SenderClientId);
        animator.SetBool("isRunning", isRun);
    }
    
    [ClientRpc]
    public void SetPosClientRPC(Vector3 pos)
    {
        Logger.Log("SetPosClientRPC called");
        transform.position = pos;
    }
}
