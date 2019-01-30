using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AI_STATE
{
    Idle = 1,
    Dash = 2,
    Walk = 3,
    Atk = 4,
    Gather_Atk = 5,
    Guard = 6,
    Back_Jump = 7,
    Wait_Attack,
    Ultimate_State
}

public class AI_IEnumrator_Func : MonoBehaviour
{
    GameMng gameMng;
    private void Start()
    {
        gameMng = GameObject.Find("GameMng").GetComponent<GameMng>();
    }

    public Fox_AI AI;


    public void Wait_Attack_Func()
    {
        StartCoroutine(Wait_Attack());
    }
    //Wait Attack
    IEnumerator Wait_Attack()
    {
        float time = 0f;
        AI.GetComponent<Player>().isGuard = true;
        AI.Ani.SetTrigger("IsIdle");
        while (AI.AM.GetDistance() > 3f && time < 3)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine("Atk_Delay");
        //랜덤공격 꼬리치기나 콤보나 평타
        Debug.Log("PLZ RAMDOM ATK");

        AI.GetComponent<Player>().isGuard = false;
    }
    //

    public void Guard_Func()
    {
        StartCoroutine(Guard());
    }

    public void Count_Func()
    {
        StartCoroutine(Count());
    }
    //일반공격
    IEnumerator Count()
    {
        AI.Ani.SetTrigger("IsIdle");

        AI.GetComponent<Player>().isGuard = true;

        yield return new WaitForSeconds(0.7f);

        AI.GetComponent<Player>().isGuard = false;
        //랜덤공격 꼬리치기나 콤보나 평타


        StartCoroutine("Atk_Delay");
    } //딜레이준뒤에
    IEnumerator Atk_Delay()
    {
        AI.Ani.SetTrigger("WeekPunch");
        yield return new WaitForSeconds(0.3f);
        AI.Ani.SetBool("b_NextPunch", true);
        AI.Ani.SetTrigger("WeekPunch");
        yield return new WaitForSeconds(0.3f);
        AI.Ani.SetTrigger("IsIdle");
        AI.Ani.SetBool("b_NextPunch", false);
        AI._Fox.curState = AI_STATE.Idle;
    }  //공격
       //   //

    //가드
    IEnumerator Guard()
    {
        AI.Ani.SetBool("IsMove", true);
        AI.Ani.SetTrigger("IsMoveFirst");
        AI.GetComponent<Player>().isGuard = true;
        yield return new WaitForSeconds(0.7f);
        AI.GetComponent<Player>().isGuard = false;
        AI._Fox.curState = AI_STATE.Idle;
    }
    //

    //점프
    IEnumerator Jump()
    {
        Vector3 moveVec3 = AI.myRb.velocity;
        if (AI.transform.position.x > 7.5f || AI.transform.position.x < -7.5f)
        {
            moveVec3 = new Vector3(AI.AM.GetMoveSpeed() * Time.deltaTime, 10, 0); // 기본 값 * 점프력
        }
        else
        {
            moveVec3 = new Vector3(AI.AM.GetMoveSpeed() * Time.deltaTime, 10, 0); // 기본 값 * 점프력
        }

        AI.myRb.velocity = (moveVec3);
        yield return new WaitForSeconds(2f);
        AI._Fox.curState = AI_STATE.Idle;
    }
    public void Jump_Func()
    {
        StartCoroutine(Jump());
    }


    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("SetWhiteScale", 0.5f);

        yield return new WaitForSeconds(1.25f);
        GameMng.Instance.camCon.isAction = false;

     //   test2.gameObject.active = true;
        gameMng.SetStart(true);
        AI.Ani.SetTrigger("궁극기");
        AI.GetComponent<Fox>().isCatch = true;
        AI.GetComponent<Fox>().catchAction = 1001;
        StartCoroutine("FixingVelocity", 0.5f);
        gameMng.camCon.SetDeFaultState(0);
        gameMng.camCon.SetEyeAdaptation(30, 0.25f);

        yield return new WaitForSeconds(0.5f);
        StartCoroutine("SetWhiteScale", 0.5f);

        yield return new WaitForSeconds(1.25f);
    }

    IEnumerator SetWhiteScale(float _Time)
    {
        float tempScale = 0;

        while (tempScale != 1)
        {
            tempScale = Mathf.MoveTowards(tempScale, 1, 1 / ((_Time / 2) * 60));
            AI.GetComponent<SpriteRenderer>().material.SetFloat("whiteScale", tempScale);
            yield return new WaitForEndOfFrame();
        }
        while (tempScale != 0)
        {
            tempScale = Mathf.MoveTowards(tempScale, 0, 1 / ((_Time / 2) * 60));
            AI.GetComponent<SpriteRenderer>().material.SetFloat("whiteScale", tempScale);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FixingVelocity(float _Time)
    {
        while (_Time >= 0)
        {
            AI.myRb.velocity = new Vector2(-15 * AI.GetComponent<Fox>().GetArrow(), AI.myRb.velocity.y);
            yield return new WaitForEndOfFrame();
            _Time -= Time.deltaTime;
        }
    }

    public void Ultimate_Func()
    {
        Debug.Log("ULTIMATE");
        AI.GetComponent<Fox>().isAction = true;
        gameMng.SetStart(false);
        gameMng.camCon.ZoomCharacter(AI.GetComponent<Fox>().isturn, AI.GetComponent<Fox>().ctrlType);
        gameMng.camCon.isAction = true;
        gameMng.camCon.SetEyeAdaptation(80, 0.1f);
        StartCoroutine("DelayStart");
        AI.Ani.SetTrigger("IsIdle");
    }



}
