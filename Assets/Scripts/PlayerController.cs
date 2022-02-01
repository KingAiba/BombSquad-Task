using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector3 moveDirection;
    private movementController playerMovementController;
    private BomberScript playerBomberScript;

    public bool isSprinting = false;
    public bool RagdollEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GetComponent<movementController>();
        playerBomberScript = GetComponent<BomberScript>();
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();

        if (playerBomberScript.isDead && !RagdollEnabled)
        {
            EnableRagdoll();
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        playerMovementController.moveObject(moveDirection, isSprinting);
        playerMovementController.rotateObject(moveDirection);
    }

    public void SetMovementDirection()
    {
        //playerMovementController.moveDirection = moveDirection;
    }

    public void getInput()
    {

        /*        if(Input.GetKey(KeyCode.Mouse0))
                {
                    float horizontalInput = Input.GetAxis("Mouse X");
                    float verticalInput = Input.GetAxis("Mouse Y");

                    Debug.Log(Input.mousePosition);
                    if (horizontalInput != 0 && verticalInput != 0)
                    {
                        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                    }
                    else if (horizontalInput != 0)
                    {
                        moveDirection = new Vector3(horizontalInput, 0, moveDirection.z).normalized;
                    }
                    else if(verticalInput != 0)
                    {
                        moveDirection = new Vector3(moveDirection.x, 0, verticalInput).normalized;
                    }

                    //moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                }
                else
                {
                    moveDirection = new Vector3(0, 0, 0);
                }*/

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        SetMovementDirection();
        ThrowBomb();
        MeleeAttack();
        Sprint();

    }

    public void ThrowBomb()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerBomberScript.ThrowBomb(transform.forward);
        }
    }

    public void MeleeAttack()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerBomberScript.MeleeAttack();
        }      
    }

    public void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    public void DisableRagdoll()
    {
        Rigidbody[] rbArr = GetComponentsInChildren<Rigidbody>();
        Collider[] cArr = GetComponentsInChildren<Collider>();

        foreach(Collider c in cArr)
        {
            if (c.gameObject.name == "Player")
            {
                continue;
            }
            else
            {
                c.enabled = false;
            }
        }

        foreach (Rigidbody rb in rbArr)
        {
            if (rb.gameObject.name == "Player")
            {
                continue;
            }
            else 
            {
                rb.isKinematic = true;
            }
        }
        RagdollEnabled = false;
    }

    public void EnableRagdoll()
    {
        Rigidbody[] rbArr = GetComponentsInChildren<Rigidbody>();
        Collider[] cArr = GetComponentsInChildren<Collider>();

        foreach (Collider c in cArr)
        {

            c.enabled = true;
            
        }
        foreach (Rigidbody rb in rbArr)
        {
            rb.isKinematic = false;
        }
        RagdollEnabled = true;
    }

}
