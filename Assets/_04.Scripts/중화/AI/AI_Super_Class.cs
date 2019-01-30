using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum states
//{
//    Idle,
//    Dash,
//    Walk,
//    Atk,
//    Gather_Atk,
//    Guard,
//    Back_Jump,
//    Wait_Attack,
//    Ultimate,
//    Tec1,
//    Tec2,
//}

public class AI_Super_Class
{
    private Strategy_Atk_AI _Atk;
    private Strategy_Move_AI _Move;

    public AI_STATE curState;

    public AI_Super_Class()
    {
        _Atk = null;
        _Move = null;
    }

    public void Attack()
    {
        _Atk.Atk();
    }
 



    public void Move(Animator a)
    {
        _Move.Move(a);
    }
  

    public void SetmoveStrategy(Strategy_Move_AI move)
    {
        _Move = move;
    }
    public void SetAtkStrategy(Strategy_Atk_AI Atk)
    {
        _Atk = Atk;
    }

    public Strategy_Move_AI GetmoveStrategy()
    {
        return _Move;
    }

    public Strategy_Atk_AI GetatkStrategy()
    {
        return _Atk;
    }
}
