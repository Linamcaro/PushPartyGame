using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnManager : MonoBehaviour
{

    public static PowerUpSpawnManager Instance { get; private set; }


    [SerializeField] private PowerUpsListSO PowerUpsListSO;


    /*[SerializeField] private Transform spawnedObjectPrefab;


    private void Update()
    {
        Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
        //spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

    }*/
}
