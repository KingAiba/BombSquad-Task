using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBomb : BombScript
{
    public GameObject miniBombPrefab;
    public int miniBombAmount;

    public override void OnDestroy()
    {
        SpawnMiniBombs();
        base.OnDestroy();
        //SpawnMiniBombs();   
    }

    private void SpawnMiniBombs()
    {
        for (int i = 0; i < miniBombAmount; i++)
        {
            GameObject spawnedBomb = Instantiate(miniBombPrefab, transform.position, miniBombPrefab.transform.rotation);
            spawnedBomb.GetComponent<Rigidbody>().velocity = bombRB.velocity;
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        
        Destroy(gameObject);
        
    }


}
