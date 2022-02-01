using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : BombScript
{

    public GameObject stuckObject = null;
    public Vector3 stickLocation = new Vector3(0, 0, 0);

    public override void Update()
    {
        base.Update();
        StickyUpdate();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bombRB.isKinematic = true;
        }
        else if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")))
        {
            StickTo(collision.GetContact(0).point, collision.gameObject);
        }
    }

    private void StickyUpdate()
    {
        if (bombType == BombTypes.Sticky && stuckObject != null)
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
        Debug.Log(position);
    }

}
