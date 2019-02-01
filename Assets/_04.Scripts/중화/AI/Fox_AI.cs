using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox_AI : MonoBehaviour
{
    public Fox__ _Fox = new Fox__();

    public AI_Manager AM;
    public Animator Ani;
    public Rigidbody2D myRb;

    private void Start()
    {
        if (!GetComponent<Player>().isAI)
            return;
        StartCoroutine(Delay());
        Ani = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Update__());
    }
    IEnumerator Update__()
    {
      
        while (true)
        {
            UpdateFox();
            Debug.Log(_Fox.curState);
            yield return new WaitForSeconds(0.3f);
        }
    }
    void UpdateFox()
    {
        if (_Fox.curState == AI_STATE.Atk ||
            _Fox.curState == AI_STATE.Guard ||
            _Fox.curState == AI_STATE.Back_Jump ||
            _Fox.curState == AI_STATE.Wait_Attack ||
            _Fox.curState == AI_STATE.Gather_Atk||
            _Fox.curState == AI_STATE.Ultimate_State||
            _Fox.curState == AI_STATE.Skill_1_State||
            _Fox.curState == AI_STATE.Skill_2_State||
            _Fox.curState == AI_STATE.Combo_1_State)
            return;
      
        if (AM.GetDistance() > 8.5f)
        {
            if (_Fox.curState == AI_STATE.Dash)
                return;
            int RandNum = Random.Range(0, 2);

            //랜덤넘
            switch (RandNum)
            {
                case 0:
                    if (_Fox.curState != AI_STATE.Dash)
                    {
                        _Fox.curState = AI_STATE.Dash;
                        _Fox.SetmoveStrategy(new Run_Strategy());
                        _Fox.Move(Ani);
                    }
                    break;
                case 1:
                    if(_Fox.curState != AI_STATE.Ultimate_State)
                    {
                        _Fox.curState = AI_STATE.Ultimate_State;
                        _Fox.SetAtkStrategy(new Ultimate_Atk());
                        _Fox.Attack();
                    }
                    break;
            }

         
            //궁극기
        }
        else if (AM.GetDistance() > 4.5f)
        {
            if (_Fox.curState == AI_STATE.Walk)
                return;

            int RandNum = Random.Range(0, 4);

            switch (RandNum)
            {
                case 0:
                    if (_Fox.curState != AI_STATE.Walk)
                    {
                        _Fox.curState = AI_STATE.Walk;
                        _Fox.SetmoveStrategy(new Walk_Strategy());
                        _Fox.Move(Ani);
                    }
                    break;
                case 1:
                    if (_Fox.curState != AI_STATE.Back_Jump)
                    {
                        _Fox.curState = AI_STATE.Back_Jump;
                        _Fox.SetmoveStrategy(new BackJump_Strategy());
                        _Fox.Move(Ani);
                    }
                    break;
                case 2:
                    if (_Fox.curState != AI_STATE.Wait_Attack)
                    {
                        _Fox.curState = AI_STATE.Wait_Attack;
                        _Fox.SetAtkStrategy(new WaitAtk_Strategy());
                        _Fox.Attack();
                    }
                    break;
                case 3:
                    if(_Fox.curState != AI_STATE.Skill_2_State)
                    {
                        _Fox.curState = AI_STATE.Skill_2_State;
                        _Fox.SetAtkStrategy(new Skill_2());
                        _Fox.Attack();
                    }
                    break;
                    //랜덤넘증가 굴러가는기술
            }
        }
        else
        {
            int RandNum = Random.Range(0, 5);
            switch (RandNum)
            {
                case 0:
                    if (_Fox.curState != AI_STATE.Gather_Atk)
                    {
                        _Fox.curState = AI_STATE.Gather_Atk;
                        _Fox.SetAtkStrategy(new Gather_Atk());
                        _Fox.Attack();
                    }
                    break;
                case 1:
                    if (_Fox.curState != AI_STATE.Guard)
                    {
                        _Fox.curState = AI_STATE.Guard;
                        _Fox.SetAtkStrategy(new Guard());
                        _Fox.Attack();
                    }
                    break;
                case 2:
                    if (_Fox.curState != AI_STATE.Wait_Attack)
                    {
                        _Fox.curState = AI_STATE.Wait_Attack;
                        _Fox.SetAtkStrategy(new WaitAtk_Strategy());
                        _Fox.Attack();
                    }
                    break;
                case 3:
                    if (_Fox.curState != AI_STATE.Skill_1_State)
                    {
                        _Fox.curState = AI_STATE.Skill_1_State;
                        _Fox.SetAtkStrategy(new Skill_1());
                        _Fox.Attack();
                    }
                    break;
                case 4:
                    if (_Fox.curState != AI_STATE.Combo_1_State)
                    {
                        _Fox.curState = AI_STATE.Combo_1_State;
                        _Fox.SetAtkStrategy(new Combo_1());
                        _Fox.Attack();
                    }
                    break;

                    //랜덤넘 증가 꼬리치기공격 or 꼬리치기 콤보
            }


        }
    }

   
}
