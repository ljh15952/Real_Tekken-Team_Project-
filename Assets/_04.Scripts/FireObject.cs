using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour {

    public CtrlType type;

    public Rigidbody2D myRb;

    public float movespeed;
    public int damage = 300;
    bool isHit;

    public FireObject SetCtrlType(CtrlType _input)
    {
        type = _input;
        return this;
    }

    public void FireRight()
    {
        myRb.velocity = new Vector2(movespeed, 0);
    }

    public void FireLeft()
    {
        myRb.velocity = new Vector2(-movespeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Damage") && !isHit)
        {
            Debug.Log(coll.gameObject.name);
            coll.GetComponent<DamageColl>().CallPlayerDamageCheck(GetComponent<Collider2D>(), damage);
            isHit = true;
        }
    }
}
