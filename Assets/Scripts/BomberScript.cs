using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour
{
    public float maxHp;
    public float currHp;

    public bool isDead = false;

    public float throwForce = 10;

    public GameObject defaultBomb;
    public GameObject pickupBomb = null;

    public float thrownCooldown = 2f;
    public bool canThrow = true;

    public BombTypes curType = BombTypes.Default;

    public int mineUses = 1;
    public int curMineUses = 0;

    public int stickyUses = 3;
    public int currStickyUses = 0;

    public float multiDuration = 10;

    // Start is called before the first frame update
    void Start()
    {
        currHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowBomb(Vector3 throwDirection)
    {
        if(canThrow == true)
        {
            if (pickupBomb == null)
            {
                UseDefault(throwDirection);
            }
            else
            {
                UseEquiped(throwDirection);
            }
            canThrow = false;
            StartCoroutine(ThrowCooldownTimer());
        }

    }

    public void UseDefault(Vector3 throwDirection)
    {
        GameObject bomb = Instantiate(defaultBomb, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
        bomb.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    public void UseEquiped(Vector3 throwDirection)
    {
        GameObject bomb = Instantiate(pickupBomb, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
        bomb.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);

        UpdateBombUses();
    }

    public void UpdateBombUses()
    {
        BombScript eBomb = pickupBomb.GetComponent<BombScript>();

        if (eBomb.bombType == BombTypes.Sticky)
        {
            currStickyUses -= 1;
            if (currStickyUses <= 0)
            {
                Unequip();
            }
        }
        else if (eBomb.bombType == BombTypes.Mine)
        {
            curMineUses -= 1;
            if(curMineUses <= 0)
            {
                Unequip();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currHp -= damage;
        if(currHp <= 0)
        {
            currHp = 0;
            isDead = true;          
        }
        else if(currHp > maxHp)
        {
            currHp = maxHp;
        }
    }

    public void Equip(GameObject bomb)
    {
        pickupBomb = bomb;

        BombScript eBomb = pickupBomb.GetComponent<BombScript>();
        if(eBomb.bombType == BombTypes.Multi)
        {
            StartCoroutine(MultiBombDurationTimer());
        }
        else if(eBomb.bombType == BombTypes.Sticky)
        {
            
            currStickyUses = stickyUses;
        }
        else if (eBomb.bombType == BombTypes.Mine)
        {
            curMineUses = mineUses;
        }

    }

    public void Unequip()
    {
        pickupBomb = null;
    }

    IEnumerator MultiBombDurationTimer()
    {
        yield return new WaitForSeconds(multiDuration);
        Unequip();
    }

    IEnumerator ThrowCooldownTimer()
    {
        yield return new WaitForSeconds(thrownCooldown);
        canThrow = true;
    }
    
}
