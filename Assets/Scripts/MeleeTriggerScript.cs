using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTriggerScript : MonoBehaviour
{
    public float meleeDamage = 2;
    public bool inMeleeRange = false;
    public BomberScript hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")))
        {
            inMeleeRange = true;
            hit = other.gameObject.GetComponent<BomberScript>();
        }
        else
        {
            inMeleeRange = false;
            hit = null;
        }
    }
}
