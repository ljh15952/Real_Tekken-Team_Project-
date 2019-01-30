using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy_Move_AI : MonoBehaviour
{
    protected GameObject A;

    public abstract void Move(Animator a);
}

class Walk_Strategy : Strategy_Move_AI
{
    public override void Move(Animator a)
    {
        a.SetBool("IsMove", true);
        a.SetTrigger("IsMoveFirst");
    }
}

class Run_Strategy : Strategy_Move_AI
{
    public override void Move(Animator a)
    {
        a.SetTrigger("IsDashFirst");
    }
}

class BackWalk_Strategy : Strategy_Move_AI
{
    public override void Move(Animator a)
    {
        Debug.Log("BackWalk DESU!!");
    }
}

class BackJump_Strategy : Strategy_Move_AI
{
    public override void Move(Animator a)
    {
        a.SetBool("IsJumpFirst", true);
        a.SetBool("IsJump", true);

        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Jump_Func();
    }
}
