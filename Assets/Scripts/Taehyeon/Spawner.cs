using Unity.Netcode;
using UnityEngine;
using Logger = Utils.Logger;

public class Spawner : SingletonNetwork<Spawner>
{
    [SerializeField] private GameObject _objectPrefab;

    [SerializeField] private int _spawnNum;
    

    public void SpawnObjects()
    {
        if(!IsServer) return;

        for (int i = 0; i < _spawnNum; i++)
        {
            NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(_objectPrefab);
            obj.transform.position = new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10));
            if (obj.TryGetComponent(out Ball ball))
            {
                ball.prefab = _objectPrefab;
            }
            
            if(!obj.IsSpawned)
                obj.Spawn(true);
        }
    }
}
