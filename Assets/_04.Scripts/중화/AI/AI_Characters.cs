using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fox__ : AI_Super_Class
{
    public Fox__()
    {
        SetmoveStrategy(new Walk_Strategy());
        SetAtkStrategy(new Gather_Atk());
    }
}

class Iyori
{

}

