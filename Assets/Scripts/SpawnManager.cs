using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefabs;

    public float spawnRangeX;
    public float spawnRangeZ;

    public int currWave = 0;
    public int enemyCount;
    public int[] waveArray;

    public GameObject[] pickupPrefabs;
    public float pickupCooldown = 5;
    public bool canSpawnPickup = true;
    public int maxPickups;

    // Start is called before the first frame update
    void Start()
    {
        waveArray = new int[] { 1, 2, 3, 4, 5 };
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        SpawnWave();
        SpawnPickup();
    }

    public Vector3 GenerateSpawnPoint()
    {
        return new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(-spawnRangeZ, spawnRangeZ));
    }

    public Vector3 GenerateSpawnPointPickup()
    {
        return new Vector3(Random.Range(-spawnRangeX, spawnRangeX), -3, Random.Range(-spawnRangeZ, spawnRangeZ));
    }

    public void SpawnWave()
    {
        if(enemyCount == 0 && currWave < waveArray.Length)
        {
            for (int i = 0; i < waveArray[currWave]; i++)
            {
                //int toSpawn = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs, GenerateSpawnPoint(), enemyPrefabs.transform.rotation);
            }
            currWave++;
        }
    }

    public void SpawnPickup()
    {
        int pickupCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        if (pickupCount <= maxPickups && canSpawnPickup)
        {
            int toSpawn = Random.Range(0, pickupPrefabs.Length);
            Instantiate(pickupPrefabs[toSpawn], GenerateSpawnPointPickup(), pickupPrefabs[toSpawn].transform.rotation);
            canSpawnPickup = false;
            StartCoroutine(PickUpCooldownTimer());
        }
    }

    IEnumerator PickUpCooldownTimer()
    {
        yield return new WaitForSeconds(pickupCooldown);
        canSpawnPickup = true;
    }

}
