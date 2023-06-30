using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;


    private void Update()
    {
        Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
        //spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

    }
}
