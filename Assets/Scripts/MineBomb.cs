using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBomb : BombScript
{


    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
