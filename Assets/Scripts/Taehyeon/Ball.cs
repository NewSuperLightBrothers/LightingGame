using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [HideInInspector] public GameObject prefab;
    private bool isAlive;
    private float TimeToDestroy = 2f;

    private void Update()
    {
        if(!IsServer) return;
        
        TimeToDestroy -= Time.deltaTime;
        
        if (TimeToDestroy < 0f)
        {
            TimeToDestroy = 2f;
            NetworkObject.Despawn();
        }
    }
}
