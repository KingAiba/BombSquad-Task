using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    AttackMelee,
    AttackBomb,
    GetPickup,
    Advance,
    Retreat,
    None,
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
    public float detectionCooldown = 5;
    public bool canDetect = true;

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
    public float mapRangeY = -10;

    public bool isRetreating = false;

    public float bombDetectionThreshold = 3;

    public EnemyState currState = EnemyState.None;

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
        if(canDetect)
        {
            DetectionPulse();
        }

        DecideState();
        LookAtTarget();
        OnDeath();
        CheckBoundY();
    }

    private void FixedUpdate()
    {
        DecideAction();
        rotateCube();
        moveCube();
    }

    public void CheckBoundY()
    {
        if(transform.position.y < mapRangeY)
        {
            Destroy(gameObject);
        }
    }

    private void LookAtTarget()
    {
        if (target == null)
        {
            //target = GameObject.Find("Player");
            canDetect = true;
            StopCoroutine(PulseCooldownTimer());
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
            if(isRetreating)
            {
                enemyMovementController.rotateObject(-moveDirection);
            }
            else
            {
                enemyMovementController.rotateObject(moveDirection);
            }
            
        }
    }

    private void moveCube()
    {
        if (movementEnabled)
        {
            if (isRetreating)
            {
                enemyMovementController.moveObject(-moveDirection);
            }
            else
            {
                enemyMovementController.moveObject(moveDirection);
            }
            
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
                if(!target.GetComponent<PickupScript>().isMine)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= minDisPickup)
                    {
                        minDisPickup = distance;
                        minPickup = target;
                    }
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

        //Debug.Log("Player : " + minDisPlayer);
        //Debug.Log("Pickup : " + minDisPickup);
        //Debug.Log("Bomb : " + minDisBomb);

        Collider newTar = PickTarget(minPlayer, minPickup, minBomb, minDisPlayer, minDisPickup, minDisBomb);
        if(newTar != null)
        {
            SetTarget(newTar.gameObject);
            canDetect = false;
            StartCoroutine(PulseCooldownTimer());
        }
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
        else if (minDisBomb != 0 && (minDisBomb < minDisPlayer) && (minDisBomb < minDisPickup) && minDisBomb < bombDetectionThreshold)
        {
            return minBomb;
        }
        else if ((minDisPickup < minDisPlayer) && (minDisPickup < minDisBomb))
        {
            return minPickup;
        }

        return minPlayer;
      
    }

    private int IsInThrowRange()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        // 1 = in range, 0 need to retreat, -1 need to advance
        if (playerDistance >= minThrowRange && playerDistance <= maxThrowRange)
        {
            return 1;
        }
        else if (playerDistance < minThrowRange)
        {
            return 0;
        }
        else if (playerDistance > maxThrowRange)
        {
            return -1;
        }

        return -1;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void ThrowBomb()
    {
        if(canThrow)
        {
            // Debug.Log(transform.rotation);
            // Debug.Log(Quaternion.LookRotation(moveDirection));
            canThrow = false;
            enemyBomberScript.ThrowBomb(transform.forward);
            StartCoroutine(ThrowCooldownTimer());
        }            
    }

    public void MeleeAttack()
    {
        if (canMelee)
        {
            canMelee = false;
            enemyBomberScript.MeleeAttack();
            StartCoroutine(MeleeCooldownTimer());
        }      
    }

    private void DecideState()
    {
        if (target == null)
        {
            ChangeState(EnemyState.None);
        }
        else if(target.CompareTag("Pickup"))
        {
            ChangeState(EnemyState.GetPickup);
        }
        else if(target.CompareTag("Bomb"))
        {
            ChangeState(EnemyState.Retreat);
        }
        else if(target.CompareTag("Player"))
        {
            if (canThrow)
            {
                int check = IsInThrowRange();
                // 1 = in range, 0 need to retreat, -1 need to advance
                if(check == 1)
                {
                    ChangeState(EnemyState.AttackBomb);
                }
                else if(check == 0)
                {
                    ChangeState(EnemyState.Retreat);
                }
                else if(check == -1)
                {
                    ChangeState(EnemyState.Advance);
                }
                else
                {
                    ChangeState(EnemyState.Advance);
                }
                
            }
            else if(canMelee)
            {
                ChangeState(EnemyState.AttackMelee);
            }
        }
    }

    private void DecideAction()
    {
        switch(currState)
        {
            case EnemyState.AttackBomb:
                isRetreating = false;
                ThrowBomb();
                rotateCube();
                break;

            case EnemyState.AttackMelee:
                isRetreating = false;
                MeleeAttack();
                rotateCube();
                moveCube();
                break;

            case EnemyState.GetPickup:
                isRetreating = false;
                rotateCube();
                moveCube();
                break;

            case EnemyState.Retreat:
                isRetreating = true;
                rotateCube();
                moveCube();
                break;

            case EnemyState.Advance:
                isRetreating = false;
                rotateCube();
                moveCube();
                break;

            case EnemyState.None:
                isRetreating = false;
                break;
        }
    }

    private void ChangeState(EnemyState state)
    {
        currState = state;
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

    IEnumerator PulseCooldownTimer()
    {
        yield return new WaitForSeconds(detectionCooldown);
        canDetect = true;
    }
}
