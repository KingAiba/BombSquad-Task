using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 15f;

    public float sprintMulti = 1.5f;

    //public Vector3 moveDirection;

    public Rigidbody objectRB;
    // Start is called before the first frame update
    void Start()
    {
        objectRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //getInput();
    }

    private void FixedUpdate()
    {
        //moveObject();
        //rotateObject();
    }

    public void moveObject(Vector3 moveDirection, bool isSprinting = false)
    {
        if(objectRB != null)
        {
            if(!isSprinting)
            {
                objectRB.AddForce(moveDirection * speed * objectRB.mass);
            }
            else
            {
                objectRB.AddForce(moveDirection * speed * sprintMulti * objectRB.mass);
            }
            
        }
    }

    public void rotateObject(Vector3 moveDirection)
    {
        if(moveDirection.magnitude == 0)
        {
            transform.rotation = transform.rotation;
        }
        else
        {
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotationSpeed);
            objectRB.MoveRotation(rotation);
        }
    }


}
