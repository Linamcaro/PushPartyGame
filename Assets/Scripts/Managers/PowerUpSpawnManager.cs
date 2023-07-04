using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Netcode.Extensions;

public class PowerUpSpawnManager : NetworkBehaviour
{
    public static PowerUpSpawnManager Instance { get; private set; }


    [SerializeField] private PowerUpsListSO PowerUpsList;

    


    private bool CanSpawnPowerUp = true;

    private void spawnPowerUp(PowerUpSO powerUpSelected)
    {
        //Set Bondaries
        float xRange = 7f;
        Vector3 zRangeInitial = LevelController.Instance.PlatformPosition();
        Vector3 zRangeFinal = LevelController.Instance.LevelLength();

        float spawnXRange = Random.Range(-xRange, xRange);
        float spawnZRange = Random.Range(zRangeInitial.z, zRangeFinal.z);
        Vector3 spawnPos = new Vector3(spawnXRange, 1, spawnZRange);

        GameObject objectToSpawn = powerUpSelected.powerUpObject;

        NetworkObject powerUp = NetworkObjectPool.Singleton.GetNetworkObject(objectToSpawn, spawnPos, Quaternion.identity);
        powerUp.Spawn(true);
        Debug.Log("PowerUp Spawned " + powerUp);


        /*GameObject spawnedObject = Instantiate(objectToSpawn, spawnPos, objectToSpawn.transform.rotation);
        //spawnedObject.GetComponent<NetworkObject>().Spawn(true);*/

    }

    private void choosePowerUp()
    {
        StartCoroutine(timer());

        int randomIndex = Random.Range(0, PowerUpsList.powerUpSOList.Count);
        Debug.Log("The amout of powerUps are: " + PowerUpsList.powerUpSOList.Count + "the random index number is: " + randomIndex);

        PowerUpSO powerUpSelected = PowerUpsList.powerUpSOList[randomIndex];

        spawnPowerUp(powerUpSelected);
    }

    IEnumerator timer()
    {
        CanSpawnPowerUp = false;
        yield return new WaitForSeconds(20);
        CanSpawnPowerUp = true;
        
    }

    private void Update()
    {
        if (!IsServer) return;
        if (!PushPartyGameManager.Instance.IsGamePlaying()) return;

        if (CanSpawnPowerUp)
        {
            choosePowerUp();
            Debug.Log("ChoosePowerUp called");


        }


    }

}
