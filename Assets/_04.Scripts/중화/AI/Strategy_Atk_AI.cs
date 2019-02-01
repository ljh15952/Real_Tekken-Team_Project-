using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy_Atk_AI : MonoBehaviour
{
    protected GameObject A;

    public abstract void Atk();
}


class Skill_1 : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Skill_1_Func();
    }
}

class Skill_2 : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Skill_2_Func();
    }
   
}

class Combo_1 : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Fox_Combo_1();
    }
}

class WaitAtk_Strategy : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Wait_Attack_Func();
    }
}

class Gather_Atk : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Count_Func();
    }
}

class Guard : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Guard_Func();
    }
}

class Ultimate_Atk : Strategy_Atk_AI
{
    public override void Atk()
    {
        A = GameObject.FindWithTag("AIF");
        A.GetComponent<AI_IEnumrator_Func>().Ultimate_Func();
    }
}