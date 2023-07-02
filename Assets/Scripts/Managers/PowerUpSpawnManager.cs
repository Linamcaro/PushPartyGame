using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpSpawnManager : MonoBehaviour
{

    public static PowerUpSpawnManager Instance { get; private set; }


    [SerializeField] private PowerUpsListSO PowerUpsList;

    //Set Bondaries
    private float xRange = 7f;
    private float zRange = 6f;

    private bool CanSpawnPowerUp = true;


    private void spawnPowerUp(PowerUpSO powerUpSelected)
    {
        


        float spawnXRange = Random.Range(-xRange, xRange);
        float spawnZRange = Random.Range(-zRange, zRange);
        Vector3 spawnPos = new Vector3(spawnXRange, 1, spawnZRange);

        GameObject objectToSpawn = powerUpSelected.powerUpObject;
        Instantiate(objectToSpawn, spawnPos, objectToSpawn.transform.rotation);

        

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
        if (CanSpawnPowerUp)
        {
            choosePowerUp();
     
        }


    }

    /*[SerializeField] private Transform spawnedObjectPrefab;


    private void Update()
    {
        Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
        //spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

    }*/
}
