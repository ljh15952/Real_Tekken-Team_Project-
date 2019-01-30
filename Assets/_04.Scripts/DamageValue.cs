using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageValue : MonoBehaviour
{

    public int damage;

    public void SetDamage(int value)
    {
        damage = value;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Damage"))
        {
            if (transform.parent.GetComponent<Player>().isCatch)
            {
                transform.parent.GetComponent<Player>().ReturnCatch();
            }
            else
                coll.GetComponent<DamageColl>().CallPlayerDamageCheck(GetComponent<Collider2D>(), damage);
        }
    }
}
