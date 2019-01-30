using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallPlayerDamageCheck(Collider2D coll, int Damage = 100)
    {
        transform.parent.GetComponent<Player>().DamageCheck(coll, name, Damage);
    }
}
