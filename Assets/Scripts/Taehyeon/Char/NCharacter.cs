using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;
using Random = UnityEngine.Random;

public class NCharacter : NetworkBehaviour
{
    public Joystick joystick;

    public NetworkVariable<myStruct> nint = new NetworkVariable<myStruct>(new myStruct(), NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public struct myStruct : INetworkSerializable
    {
        public int aint;
        public bool bbool;
        public NetworkString sss;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref aint);
            serializer.SerializeValue(ref bbool);
            serializer.SerializeValue(ref sss);
        }
    }

    public override void OnNetworkSpawn()
    {
        nint.OnValueChanged += ((value, newValue) =>
        {
            Logger.Log(OwnerClientId + " / aint : " +  nint.Value.aint + " / bbool : " + nint.Value.bbool + " / sss : " + nint.Value.sss);
        });
    }

    private void Update()
    {
        if (joystick != null)
        {
            if (IsOwner)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // nint.Value = new myStruct
                    // {
                    //     aint = Random.Range(0, 100),
                    //     bbool = !nint.Value.bbool,
                    //     sss = nint.Value.sss + "1"
                    // };
                    
                    TestServerRPC(new ServerRpcParams());
                }

                // Logger.Log("player id : " + OwnerClientId + " random val : " + nint.Value);
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

    [ServerRpc]
    public void TestServerRPC(ServerRpcParams rpcParams)
    {
        Logger.Log(OwnerClientId + " / " + rpcParams.Receive.SenderClientId);
    }
}
