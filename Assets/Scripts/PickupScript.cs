using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public GameObject assignedBomb;

    public float rotationSpeed = 10;

    public bool isHeal = false;
    public bool isMine = false;
    public float healAmount = 5;
    // Start is called before the first frame update
    void Start()
    {
        ChangePickupColor();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePickup();
    }

    public void GivePickup(BomberScript target)
    {
        target.Equip(assignedBomb);
        Debug.Log(target.tag);
    }

    public void ChangePickupColor()
    {
        if(isHeal)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }      
    }

    public void RotatePickup()
    {
        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + (Time.deltaTime * rotationSpeed), transform.rotation.z));

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
