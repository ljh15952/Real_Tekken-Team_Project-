using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI_Manager : MonoBehaviour
{

    public GameObject P2_AI;
    public GameObject P1;

    public float distance;

    public float moveSpeed;

    public float GetDistance()
    {
        distance = P2_AI.transform.position.x - P1.transform.position.x;
        return distance > 0 ? distance : -distance;
    }

    public float GetMoveSpeed()
    {
        return distance > 0 ? moveSpeed : -moveSpeed;
    }
   
}
