using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombTypes
{ 
    Default,
    Multi,
    Sticky,
    Mine,
}

public class BombScript : MonoBehaviour
{

    public BombTypes bombType;

    public float ttl;
    public float damage;
    public float explosionRadius;
    public float damageFalloff;
    public float explosionForce;

    public Rigidbody bombRB;

    public GameObject stuckObject = null;
    public Vector3 stickLocation = new Vector3(0, 0, 0);

    public GameObject miniBombPrefab;
    public int miniBombAmount;

    public ParticleSystem explosion;
    public float explosionDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        bombRB = GetComponent<Rigidbody>();
        StartCoroutine(BombTimer());
    }

    // Update is called once per frame
    private void Update()
    {
        StickyUpdate();
    }

    private void OnDestroy()
    {
        DamageObjectsInRange();

        ParticleSystem exp = Instantiate(explosion, transform.position, explosion.transform.rotation);
        Destroy(exp, explosionDuration);

        if (bombType == BombTypes.Multi)
        {
            SpawnMiniBombs();
        }
    }

    public float CalcDamage()
    {
        return damage;
    }

    public void ExplosiveForce(Rigidbody otherRB)
    {
        if (otherRB.GetComponent<BomberScript>().isDead)
        {
            otherRB.AddExplosionForce(explosionForce * 5, transform.position, explosionRadius, 0f, ForceMode.Impulse);
        }
        else
        {
            otherRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
        }
        
    }

    public void DamageObjectsInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in hitColliders)
        {
            if(hit.CompareTag("Enemy") || hit.CompareTag("Player"))
            {
                hit.GetComponent<BomberScript>().TakeDamage(CalcDamage());
                ExplosiveForce(hit.GetComponent<Rigidbody>());
            }
        }
    }

    private void StickyUpdate()
    {
        if(bombType == BombTypes.Sticky && stuckObject != null)
        {
            transform.position = stuckObject.transform.position - stickLocation;
        }
    }

    private void StickTo(Vector3 position, GameObject obj)
    {
        bombRB.isKinematic = true;

        stuckObject = obj;
        stickLocation = obj.transform.position - position;

        Destroy(GetComponent<SphereCollider>());
        //Debug.Log(position);

    }

    private void SpawnMiniBombs()
    {   
        for (int i=0; i<miniBombAmount; i++)
        {         
            GameObject spawnedBomb = Instantiate(miniBombPrefab, transform.position, miniBombPrefab.transform.rotation);
            spawnedBomb.GetComponent<Rigidbody>().velocity = bombRB.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (bombType == BombTypes.Sticky || bombType == BombTypes.Mine))
        {
            bombRB.isKinematic = true;
        }
        else if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) && bombType == BombTypes.Sticky)
        {
            StickTo(collision.GetContact(0).point, collision.gameObject);
        }
        else if (bombType == BombTypes.Multi)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && bombType == BombTypes.Mine)
        {
            Destroy(gameObject);
        }
    }

     IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(ttl);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
