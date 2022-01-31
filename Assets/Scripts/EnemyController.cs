using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    AttackMelee,
    AttackBomb,
    Retreat,
}

public class EnemyController : MonoBehaviour
{
    public Vector3 moveDirection;

    public GameObject target;
    public GameObject player;

    public bool rotationEnabled = true;
    public bool movementEnabled = true;
    public bool attackEnabled = true;

    private movementController enemyMovementController;
    private BomberScript enemyBomberScript;

    public float detectionRadius = 8;

    public float playerBias = 1;
    public float pickupBias = 1;
    public float bombBias = 1;

    public float throwStrength;
    public float throwCooldown;
    public bool canThrow = true;
    public float minThrowRange;
    public float maxThrowRange;

    public float meleeRange;
    public float meleeCooldown;
    public bool canMelee = true;

    public float mapRangeX = 14;
    public float mapRangeZ = 11;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovementController = GetComponent<movementController>();
        enemyBomberScript = GetComponent<BomberScript>();

        player = GameObject.Find("Player");
        DetectionPulse();
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

    private void DetectionPulse()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, detectionRadius);

        Collider minPlayer = null;
        Collider minPickup = null;
        Collider minBomb = null;

        float minDisPickup = Mathf.Infinity;
        float minDisPlayer = Mathf.Infinity;
        float minDisBomb = Mathf.Infinity;

        foreach(Collider target in targets)
        {
            //Debug.Log(target.tag);           
            if(target.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if(distance <= minDisPlayer)
                {
                    minDisPlayer = distance;
                    minPlayer = target;
                }
            }
            else if(target.CompareTag("Pickup"))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= minDisPickup)
                {
                    minDisPickup = distance;
                    minPickup = target;
                }
            }
            else if(target.CompareTag("Bomb"))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= minDisBomb)
                {
                    minDisBomb = distance;
                    minBomb = target;
                }
            }
        }

        Debug.Log("Player : " + minDisPlayer);
        Debug.Log("Pickup : " + minDisPickup);
        Debug.Log("Bomb : " + minDisBomb);

        Collider newTar = PickTarget(minPlayer, minPickup, minBomb, minDisPlayer, minDisPickup, minDisBomb);
        SetTarget(newTar.gameObject);

    }

    private Collider PickTarget(Collider minPlayer, Collider minPickup, Collider minBomb, float minDisPlayer, float minDisPickup, float minDisBomb)
    {
        minDisPlayer *= playerBias;
        minDisPickup *= pickupBias;
        minDisBomb *= minDisBomb;

        if ((minDisPlayer < minDisBomb) && (minDisPlayer < minDisPickup))
        {
            return minPlayer;
        }
        else if (minDisBomb != 0 && (minDisBomb < minDisPlayer) && (minDisBomb < minDisPickup))
        {
            return minBomb;
        }
        else if ((minDisPickup < minDisPlayer) && (minDisPickup < minDisBomb))
        {
            return minPickup;
        }

        return minPlayer;
      
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void ThrowBomb()
    {
        if(canThrow)
        {
            enemyBomberScript.ThrowBomb(transform.forward);
        }            
    }

    public void MeleeAttack()
    {
        if (canMelee)
        {
            enemyBomberScript.MeleeAttack();
        }      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    IEnumerator ThrowCooldownTimer()
    {
        yield return new WaitForSeconds(throwCooldown);
        canThrow = true;
    }

    IEnumerator MeleeCooldownTimer()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMelee = true;
    }
}
