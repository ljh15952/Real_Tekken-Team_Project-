using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public abstract class Strategy_Atk
//{
//    public abstract void Atk();
//}

//class Normal_Atk : Strategy_Atk
//{
//    public override void Atk()
//    {
//        Debug.Log("Normal Atk DESU!");
//    }
//}

//class Skill_1 : Strategy_Atk
//{
//    public override void Atk()
//    {
//        Debug.Log("SKILL_1 DESU!");
//    }
//}

//class Skill_2 : Strategy_Atk
//{
//    public override void Atk()
//    {
//        Debug.Log("SKILL_2 DESU!");
//    }
//}

//class Combo_1 : Strategy_Atk
//{
//    public override void Atk()
//    {
//        Debug.Log("Combo_1 DESU!");
//    }
//}

//class WaitAtk_Strategy : Strategy_Atk
//{
//    public override void Atk()
//    {
//        Debug.Log("WAIT ATK");
//    }
//}

//public abstract class Strategy_Move : MonoBehaviour
//{
//    public abstract void Move(Animator a);
//    public abstract void Move(Animator a, Rigidbody2D b);
//    public abstract void Move(Animator a, Rigidbody2D b, AI_Manager AM);
//    public abstract void Move(Animator a, Rigidbody2D b, AI_Manager AM,GameObject G);

//}

//class Walk_Strategy : Strategy_Move
//{
//    public override void Move(Animator a)
//    {
//        a.SetBool("IsMove", true);
//        a.SetTrigger("IsMoveFirst");
//    }
//    public override void Move(Animator a, Rigidbody2D rb)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM,GameObject G)
//    {
//    }
//}

//class Run_Strategy : Strategy_Move
//{
//    public override void Move(Animator a)
//    {
//        a.SetTrigger("IsDashFirst");
//    }
//    public override void Move(Animator a, Rigidbody2D rb)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM, GameObject G)
//    {
//    }
//}

//class BackWalk_Strategy : Strategy_Move
//{
//    public override void Move(Animator a)
//    {
//        Debug.Log("BackWalk DESU!!");
//    }
//    public override void Move(Animator a, Rigidbody2D rb)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM, GameObject G)
//    {
//    }
//}

//class BackJump_Strategy : Strategy_Move
//{
//    public override void Move(Animator a)
//    {
//        a.SetBool("IsJumpFirst", true);
//        a.SetBool("IsJump", true);
//    }
//    public override void Move(Animator a, Rigidbody2D rb)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM)
//    {
//    }
//    public override void Move(Animator a, Rigidbody2D rb, AI_Manager AM, GameObject G)
//    {
      
//    }
//}

