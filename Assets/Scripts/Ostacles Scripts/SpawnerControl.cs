using Netcode.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnerControl : NetworkBehaviour
{
    public static SpawnerControl Instance;

    [SerializeField] private GameObject prefab;

    [SerializeField] private int maxObjects;

    private void Start()
    {
        if (Instance == null)
        {

            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Singleton.InitializePool();
        };

    }

    public void SpawnObject()
    {
        if (!IsServer) return;

        for(int i = 0; i < maxObjects; i++)
        {
            //GameObject go = Instantiate(prefab, 
            //    new Vector3(Random.Range(-10, 10), 10f, Random.Range(-10, 10)), Quaternion.identity);
            GameObject go = NetworkObjectPool.Singleton.GetNetworkObject(prefab).gameObject;
            go.transform.position = new Vector3(Random.Range(-10, 10), 10f, Random.Range(-10, 10));
            go.GetComponent<NetworkObject>().Spawn();
        }

    }
}
