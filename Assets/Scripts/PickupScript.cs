using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public GameObject assignedBomb;

    public bool isHeal = false;
    public bool isMine = false;
    public float healAmount = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GivePickup(BomberScript target)
    {
        target.Equip(assignedBomb);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy") && !(other.gameObject.CompareTag("Enemy") && assignedBomb.GetComponent<BombScript>().bombType == BombTypes.Mine))
        {
            if(!isHeal)
            {
                GivePickup(other.GetComponent<BomberScript>());           
            }
            else
            {
                other.GetComponent<BomberScript>().TakeDamage(-healAmount);
            }
            Destroy(gameObject);
        }
    }

}
