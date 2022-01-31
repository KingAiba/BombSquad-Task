using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Vector3 moveDirection;
    private movementController playerMovementController;
    private BomberScript playerBomberScript;

    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GetComponent<movementController>();
        playerBomberScript = GetComponent<BomberScript>();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        

        ThrowBomb();
    }

    private void FixedUpdate()
    {
        playerMovementController.moveObject(moveDirection);
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

    }

    public void ThrowBomb()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerBomberScript.ThrowBomb(transform.forward);
        }
    }
    
}
