using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Fox : Player {

    public int catchAction;
    public Material test;
    public ParticleSystem test2;
    private void Awake()
    {
        if (ctrlType == CtrlType.One)
        {
            ctrlKey.Up = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1Up", "W"));
            ctrlKey.Down = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1Down", "S"));
            ctrlKey.Left = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1Left", "A"));
            ctrlKey.Right = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1Right", "D"));
            ctrlKey.WeakPunch = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1WeakPunch", "J"));
            ctrlKey.PowerPunch = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1PowerPunch", "K"));
            ctrlKey.WeakKick = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1WeakKick", "U"));
            ctrlKey.PowerKick = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p1PowerKick", "I"));
        }
        // 플레이어 2p-
        else if (ctrlType == CtrlType.Two)
        {
            ctrlKey.Up = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2Up", "Keypad8"));
            ctrlKey.Down = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2Down", "Keypad5"));
            ctrlKey.Left = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2Left", "Keypad4"));
            ctrlKey.Right = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2Right", "Keypad6"));
            ctrlKey.WeakPunch = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2WeakPunch", "L"));
            ctrlKey.PowerPunch = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2PowerPunch", "Semicolon"));
            ctrlKey.WeakKick = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2WeakKick", "Period"));
            ctrlKey.PowerKick = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("p2PowerKick", "Slash"));
        }
    }
    void Start()
    {
        gameMng = GameObject.Find("GameMng").GetComponent<GameMng>();
        transform = GetComponent<Transform>();

        top = transform.GetChild(0).GetComponent<Transform>();
        middle = transform.GetChild(1).GetComponent<Transform>();
        lower = transform.GetChild(2).GetComponent<Transform>();

        myRb = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();

        uiMng = GameObject.Find("UIMng").GetComponent<UIMng>();
        effectMng = GameObject.Find("EffectMng").GetComponent<EffectMng>();

        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update () {
        SetAfterImage();
        if(Input.GetKeyDown(KeyCode.P))
            StartCoroutine("SetWhiteScale", 0.25f);

        if (gameMng.GetStart()) 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            if (!isDamage)
                PlayerControll();
            SetTimer();
        }
    }

    public override void TurnRight()
    {
        Vector3 turnVec3 = transform.localScale;
        turnVec3.x = 0.55f;
        transform.localScale = turnVec3;
    }
    public override void TurnLeft()
    {
        Vector3 turnVec3 = transform.localScale;
        turnVec3.x = -0.55f;
        transform.localScale = turnVec3;
    }
    protected override void PlayerAttack(KeyCode WeekPunch, KeyCode RiverPunch, KeyCode WeekKick, KeyCode RiverKick)
    {
        SaveCommandAttack(WeekPunch, RiverPunch, WeekKick, RiverKick);
        float aniCurrent = playerAni.GetCurrentAnimatorStateInfo(0).normalizedTime;
        bool aniName = false;
        bool noAction = false;
        if (playerAni.GetCurrentAnimatorStateInfo(0).IsName("Fox_Lower") || playerAni.GetCurrentAnimatorStateInfo(0).IsName("Fox_LowerRevease"))
            noAction = true;
        if (playerAni.GetCurrentAnimatorStateInfo(0).IsName("Fox_Punch1_1") || playerAni.GetCurrentAnimatorStateInfo(0).IsName("Fox_Punch1_2"))
            aniName = true;

        if (!isLower && !isJump && !noAction)
        {
            if (Input.GetKeyDown(WeekPunch) && weekPunchCount < 2)
            {
                if ((aniName) && aniCurrent > 0.95f) // 예외 처리(안하면 버그 발생)
                    return;
                if (!isAction)
                {
                    SetDamage(50);
                    playerAni.SetTrigger("WeekPunch");
                    isAction = true;
                    playerAni.SetBool("IsAction", isAction);
                }
                weekPunchCount += 1;
                playerAni.SetInteger("PunchCount", weekPunchCount);
            }
            if (Input.GetKeyDown(RiverPunch))
            {
            }
            if (Input.GetKeyDown(WeekKick))
            {
            }
            if (Input.GetKeyDown(RiverKick))
            {
            }
        }
        else if (isLower)
        {
            if (Input.GetKeyDown(WeekPunch))
            {
                if (!isAction)
                {
                    playerAni.SetTrigger("IsLowerAttack");
                    isAction = true;
                    playerAni.SetBool("IsAction", isAction);
                }
            }
        }
        else if (isJump)
        {
            if (Input.GetKeyDown(WeekPunch))
            {
                if (!isAction)
                {
                    playerAni.SetTrigger("IsJumpWeekAttack");
                    isAction = true;
                    playerAni.SetBool("IsAction", isAction);
                }
            }
        }
    }
    protected override void PlayerTechnique(int index)
    {
        if (index == 0)
        {
            isAction = true;
            playerAni.SetTrigger("기술1");
        }
        else if (index == 1)
        {
               isAction = true;
            myRb.velocity = new Vector2(-13f * GetArrow(), 10f) ;
            playerAni.SetTrigger("기술2");
        }
        else if (index == 2)
        {
            isAction = true;
            playerAni.SetTrigger("기술3");
        }
    }
    protected override void PlayerUltimate()
    {
        isAction = true;
        gameMng.SetStart(false);
        gameMng.camCon.ZoomCharacter(isturn, ctrlType);
        gameMng.camCon.isAction = true;
        gameMng.camCon.SetEyeAdaptation(80, 0.1f);
        StartCoroutine("DelayStart");
        playerAni.SetTrigger("IsIdle");
    }
    public override void ReturnCatch()
    {
        if(catchAction == 1001)
        {
            SetDamage(35);
            playerAni.SetTrigger("궁극기_패기");
            myRb.velocity = new Vector2(0, 0);
            
        StartCoroutine("FixingPosition", 1.5f);
            gameMng.camCon.shake += 1.5f;
        }
        isCatch = false;
    }


    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("SetWhiteScale", 0.5f);

        yield return new WaitForSeconds(1.25f);
        test2.gameObject.active = true;
        gameMng.SetStart(true);
        playerAni.SetTrigger("궁극기");
        isCatch = true;
        catchAction = 1001;
        StartCoroutine("FixingVelocity", 0.5f);
        gameMng.camCon.SetDeFaultState(0);
        gameMng.camCon.SetEyeAdaptation(30, 0.25f);
    }
    IEnumerator FixingVelocity(float _Time)
    {
        while (_Time >= 0)
        {
            myRb.velocity = new Vector2(-15 * GetArrow(), myRb.velocity.y);
            yield return new WaitForEndOfFrame();
            GameMng.Instance.camCon.isAction = false;
            _Time -= Time.deltaTime;
        }
    }
    IEnumerator FixingPosition(float _Time)
    {
        while (_Time >= 0)
        {
        StopCoroutine("FixingVelocity");
            transform.position = gameMng.Hero1.GetComponent<Transform>().position + new Vector3(1f * GetArrow(), 1);
            myRb.velocity = new Vector2(0, 0);
            yield return new WaitForEndOfFrame();
            _Time -= Time.deltaTime;
        }
        playerAni.SetTrigger("궁극기_뒷무빙");
        gameMng.camCon.SetDeFaultState();
        gameMng.Hero2.SetDefaultActionState();
        gameMng.Hero2.isLower = false;
        myRb.velocity = new Vector2(10 * GetArrow(), 10);
        isAction = false;
        yield return new WaitForSeconds(1);
        test2.gameObject.active = false;

    }
    public void SetAfterImage()
    {
        test.mainTexture = GetComponent<SpriteRenderer>().sprite.texture;
        //test2.textureSheetAnimation.SetSprite(0, GetComponent<SpriteRenderer>().sprite);
    }
    IEnumerator SetWhiteScale(float _Time)
    {
        float tempScale = 0;

        while (tempScale != 1)
        {
            tempScale = Mathf.MoveTowards(tempScale, 1, 1 / ((_Time / 2) * 60));
            GetComponent<SpriteRenderer>().material.SetFloat("whiteScale", tempScale);
            yield return new WaitForEndOfFrame();
        }
        while (tempScale != 0)
        {
            tempScale = Mathf.MoveTowards(tempScale, 0  , 1 / ((_Time / 2) * 60));
            GetComponent<SpriteRenderer>().material.SetFloat("whiteScale", tempScale);
            yield return new WaitForEndOfFrame();
        }
    }
}