using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 moveDirection;

    public GameObject target;

    public bool rotationEnabled = true;
    public bool movementEnabled = true;
    public bool attackEnabled = true;

    private movementController enemyMovementController;
    private BomberScript enemyBomberScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovementController = GetComponent<movementController>();
        enemyBomberScript = GetComponent<BomberScript>();

        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        LookAtTarget();
        OnDeath();
    }

    private void FixedUpdate()
    {
        rotateCube();
        moveCube();
    }

    private void LookAtTarget()
    {
        if (target == null)
        {
            target = GameObject.Find("Player");
        }
        else
        {
            moveDirection = (target.transform.position - transform.position).normalized;
        }

    }

    private void rotateCube()
    {
        if (rotationEnabled)
        {
            enemyMovementController.rotateObject(moveDirection);
        }
    }

    private void moveCube()
    {
        if (movementEnabled)
        {
            enemyMovementController.moveObject(moveDirection);
        }
    }

    private void OnDeath()
    {
        if(enemyBomberScript.isDead)
        {
            rotationEnabled = false;
            movementEnabled = false;
            attackEnabled = false;
        }
    }


}
