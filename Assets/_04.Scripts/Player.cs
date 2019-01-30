using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum CtrlType { One, Two }
[System.Serializable]
public class CtrlKey
{
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode WeakPunch;
    public KeyCode PowerPunch;
    public KeyCode WeakKick;
    public KeyCode PowerKick;
}
public class Player : MonoBehaviour
{
    public enum CommandType { left, right, up, down, punch1, punch2, kick1, kick2 }
    public enum CharacterKind { Fox, Tiger, Pander }
    protected Animator playerAni;
    protected new Transform transform;

    public CommandData commandKind;

    public GameObject fireObj;
    // 상단, 중단, 하단 포지션
    protected GameMng gameMng;

    protected Transform top;
    protected Transform middle;
    protected Transform lower;
    //

    protected Rigidbody2D myRb; // 플레이어 리지드바디

    // 스크립트
    protected UIMng uiMng;
    protected EffectMng effectMng;
    //

    public bool isAI;

    public float Hp; // 체력
    public float MaxHp; // 최대 체력

    public int weekPunchCount; // 약 펀치 누를 때 마다 카운트 올라감, 첫 번째 공격 끝나기 전에 누르면 다음 공격 실행
    public int currentActionCount; // 공격(애니메이션) 시작할 때 올라가는 카운트
                                   // 공격 끝날 때 SetPlayerAction() 함수 실행 해서 검사

    // 커맨드
    // 임시
    public Transform textMom;
    public GameObject comandText;
    public Text whatCommand;
    //
    public string command; // 커맨드 저장 공간(문자열)
    //


    public CtrlType ctrlType; // 1p, 2p 구분
    public CtrlKey ctrlKey;

    // 속도
    public float moveSpeed; // 이동 속도
    protected float dashSpeed; // 대쉬 속도

    public float jumpPower; // 점프력
    protected int jumpCount; // 점프 제한
    //

    // 상태 체크
    protected bool isDead;
    public bool isMove; // 이동 중
    public bool isJump; // 점프 중
    public bool isAction; // 액션(공격) 중
    protected bool isDamage; // 피격 중
    public bool isCatch;
    public bool isLower; // 앉아 있는 중
    //

    public bool isturn = false; //자리가 바꼈는지 안바꼈는지 체크
    public bool isGuard = false; //가드 상태인지 아닌지 체크


    public float commandTimeLimit; // 커맨드 초기화 시간
    public float commandTimer; // 커맨드 체크 타이머

    // 대쉬
    public float dashTimerLimit = 0.5f; // 대쉬 연타 초기화 시간
    public float dashTimer; // 대쉬 타이머
    public int dashCount; // 방향키 연타 횟수

    protected int isLeft; // 왼쪽 방향키 누른 횟수
    protected int isRight; // 오른쪽-
    //

    protected string p1Up;
    public string leftArrow;

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

    void Update()
    {
        if (gameMng.GetStart())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            if (!isDamage)
                PlayerControll();
            SetTimer();
        }
    }

    /// <summary>
    ///  플레이어 키 설정
    /// </summary>
    protected void PlayerControll()
    {
        // 플레이어 1p 키 설정
        if (!isAI)
        {
            PlayerAttack(ctrlKey.WeakPunch, ctrlKey.PowerPunch, ctrlKey.WeakKick, ctrlKey.PowerKick);
            Move(transform, ctrlKey.Left, ctrlKey.Right, ctrlKey.Up, ctrlKey.Down);
            //if (ctrlType == CtrlType.One)
            //{
            //    PlayerAttack(KeyCode.J, KeyCode.K, KeyCode.U, KeyCode.I);
            //    Move(transform, KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S);
            //}
            //// 플레이어 2p-
            //else if (ctrlType == CtrlType.Two)
            //{
            //    PlayerAttack(KeyCode.L, KeyCode.Semicolon, KeyCode.Period, KeyCode.Slash);
            //    Move(transform, KeyCode.Keypad4, KeyCode.Keypad6, KeyCode.Keypad8, KeyCode.Keypad5);
            //}
        }
    }

    /// <summary>
    ///  커맨드
    /// </summary>
    /// <param name="_command"></param>
    protected void CheckCommand(string _command)
    {
        commandTimer = 0;
        GameObject tempGame;
        tempGame = Instantiate(comandText);
        tempGame.GetComponent<RectTransform>().parent = textMom;
        tempGame.GetComponent<Text>().text = _command;
        command += _command;
        char[] charCommand = command.ToCharArray();
        string checkCommand = null;
        //Debug.Log(_command);
        for (int j = 0; j < 3; j++)
        {
            if (charCommand.Length >= commandKind.Technique[j].Length)
            {
                for (int i = charCommand.Length - commandKind.Technique[j].Length; i < charCommand.Length; i++)
                {
                    checkCommand += charCommand[i];
                    if ((ctrlType == CtrlType.One && !isturn) || (ctrlType == CtrlType.Two && isturn))
                    {
                        if (checkCommand == commandKind.Technique[j])
                        {
                            DeleatCommand();
                            whatCommand.text = commandKind.TechniqueName[j];
                            PlayerTechnique(j);
                        }
                    }
                    else
                    {
                        char[] tempCommandSave = checkCommand.ToCharArray();
                        string reverseCommand = null;

                        for (int l = 0; l < tempCommandSave.Length; l++)
                        {
                            if (tempCommandSave[l] == '↙')
                                tempCommandSave[l] = '↘';
                            else if (tempCommandSave[l] == '↘')
                                tempCommandSave[l] = '↙';
                            else if (tempCommandSave[l] == '←')
                                tempCommandSave[l] = '→';
                            else if (tempCommandSave[l] == '→')
                                tempCommandSave[l] = '←';
                            reverseCommand += tempCommandSave[l];
                        }

                        if (reverseCommand == commandKind.Technique[j])
                        {
                            DeleatCommand();
                            whatCommand.text = commandKind.TechniqueName[j];
                            PlayerTechnique(j);
                        }
                    }
                }
            }
            checkCommand = null;
        }
        if (charCommand.Length >= commandKind.Skill.Length)
        {
            for (int i = charCommand.Length - commandKind.Skill.Length; i < charCommand.Length; i++)
            {
                checkCommand += charCommand[i];
                if ((ctrlType == CtrlType.One && !isturn) || (ctrlType == CtrlType.Two && isturn))
                {
                    if (checkCommand == commandKind.Skill)
                    {
                        DeleatCommand();
                        whatCommand.text = commandKind.Skill;
                        PlayerSkill();
                    }
                }
                else
                {
                    char[] tempCommandSave = checkCommand.ToCharArray();
                    string reverseCommand = null;

                    for (int l = 0; l < tempCommandSave.Length; l++)
                    {
                        if (tempCommandSave[l] == '↙')
                            tempCommandSave[l] = '↘';
                        else if (tempCommandSave[l] == '↘')
                            tempCommandSave[l] = '↙';
                        else if (tempCommandSave[l] == '←')
                            tempCommandSave[l] = '→';
                        else if (tempCommandSave[l] == '→')
                            tempCommandSave[l] = '←';
                        reverseCommand += tempCommandSave[l];
                    }

                    if (reverseCommand == commandKind.Skill)
                    {
                        DeleatCommand();
                        whatCommand.text = commandKind.Skill;
                        PlayerSkill();
                    }
                }
            }
        }
        if (charCommand.Length >= commandKind.HiddenSkill.Length)
        {
            for (int i = charCommand.Length - commandKind.HiddenSkill.Length; i < charCommand.Length; i++)
            {
                checkCommand += charCommand[i];
                if ((ctrlType == CtrlType.One && !isturn) || (ctrlType == CtrlType.Two && isturn))
                {
                    if (checkCommand == commandKind.HiddenSkill)
                    {
                        DeleatCommand();
                        whatCommand.text = commandKind.HiddenSkill;
                        PlayerUltimate();
                    }
                }
                else
                {
                    char[] tempCommandSave = checkCommand.ToCharArray();
                    string reverseCommand = null;

                    for (int l = 0; l < tempCommandSave.Length; l++)
                    {
                        if (tempCommandSave[l] == '↙')
                            tempCommandSave[l] = '↘';
                        else if (tempCommandSave[l] == '↘')
                            tempCommandSave[l] = '↙';
                        else if (tempCommandSave[l] == '←')
                            tempCommandSave[l] = '→';
                        else if (tempCommandSave[l] == '→')
                            tempCommandSave[l] = '←';
                        reverseCommand += tempCommandSave[l];
                    }

                    if (reverseCommand == commandKind.HiddenSkill)
                    {
                        DeleatCommand();
                        whatCommand.text = commandKind.HiddenSkill;
                        PlayerUltimate();
                    }
                }
            }
        }
    }

    protected virtual void PlayerTechnique(int index)
    {
        if (index == 0)
        {
            Debug.Log(999);
            isAction = true;
            playerAni.SetTrigger("기술1");
        }
    }
    protected virtual void PlayerSkill()
    {
        isAction = true;
        playerAni.SetTrigger("스킬1");
    }
    protected virtual void PlayerUltimate()
    {
        isAction = true;
        playerAni.SetTrigger("궁극기");
    }

    void DeleatCommand()
    {
        for (int i = 0; i < textMom.childCount; i++)
        {
            Destroy(textMom.GetChild(i).gameObject);
        }
        command = "";
    }
    protected void SaveCommand(KeyCode left, KeyCode right, KeyCode up, KeyCode down)
    {
        //if (Input.GetKey(right) && Input.GetKeyUp(down))
        //    PrintCommand("→");
        //else if ((Input.GetKeyUp(right) && Input.GetKey(down))
        //    || (Input.GetKeyUp(left) && Input.GetKey(down)))
        //    PrintCommand("↓");
        //else if (Input.GetKey(left) && Input.GetKeyUp(down))
        //    PrintCommand("←");

        if (Input.GetKey(right) && Input.GetKeyDown(down) || Input.GetKeyDown(right) && Input.GetKey(down))
            CheckCommand("↘");
        else if (Input.GetKey(left) && Input.GetKeyDown(down) || Input.GetKeyDown(left) && Input.GetKey(down))
            CheckCommand("↙");
        else if (Input.GetKeyDown(left) || Input.GetKey(left) && Input.GetKeyUp(down))
            CheckCommand("←");
        else if (Input.GetKeyDown(right) || Input.GetKey(right) && Input.GetKeyUp(down))
            CheckCommand("→");
        if (Input.GetKeyDown(up))
            CheckCommand("↑");
        else if (Input.GetKeyDown(down))
            CheckCommand("↓");
        //else if (Input.GetKeyDown(down) || (Input.GetKey(down) && Input.GetKeyUp(right)) || (Input.GetKey(down) && Input.GetKeyUp(left)))
        //    PrintCommand("↓");
    }
    protected void SaveCommandAttack(KeyCode WeekPunch, KeyCode RiverPunch, KeyCode WeekKick, KeyCode RiverKick)
    {
        if (Input.GetKeyDown(WeekPunch))
            CheckCommand("Q");
        else if (Input.GetKeyDown(RiverPunch))
            CheckCommand("W");
        if (Input.GetKeyDown(WeekKick))
            CheckCommand("A");
        else if (Input.GetKeyDown(RiverKick))
            CheckCommand("S");
    }

    /// <summary>
    /// 이동, 점프
    /// </summary>
    /// <param name="_trans"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="up"></param>
    /// <param name="down"></param>
    public void Move(Transform _trans, KeyCode left, KeyCode right, KeyCode up, KeyCode down)
    {
        SaveCommand(left, right, up, down);

        float hor = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        float ver = Input.GetAxisRaw("Vertical") * Time.deltaTime * jumpPower;
        Vector3 moveVec3 = myRb.velocity;


        if (!isLower)
        {
            if (Input.GetKeyDown(left))
                isLeft += 1;
            if (Input.GetKeyDown(right))
                isRight += 1;

            if (Input.GetKeyDown(left) ||
                Input.GetKeyDown(right))
            {
                //Debug.Log(isLeft + " " + isRight);
                if (dashCount == 0)
                {
                    isMove = true;
                    dashSpeed = 1;

                    if (!isJump)
                    {
                        playerAni.SetTrigger("IsMoveFirst");
                        dashTimer = 0;
                        dashCount += 1;
                    }
                }
                // 시간 내에 방향키 연타시
                else if (dashCount == 1 && dashTimer <= dashTimerLimit && 
                    (isLeft >= 2 && (((isturn && ctrlType == CtrlType.One)) || (!isturn && ctrlType == CtrlType.Two)) || 
                    (isRight >= 2 && ((!isturn && ctrlType == CtrlType.One)) || (isturn && ctrlType == CtrlType.Two)))
                    && !isAction)
                {
                    isMove = true;
                    if (!isJump)
                    {
                        dashSpeed = 2;
                        playerAni.SetTrigger("IsDashFirst");
                        isLeft = 0;
                        isRight = 0;
                        dashCount = 0;
                    }
                }
                else
                {
                    isMove = true;
                    dashSpeed = 1;

                    if (!isJump)
                    {
                        playerAni.SetTrigger("IsMoveFirst");
                        isLeft = 0;
                        isRight = 0;
                        dashTimer = 0;
                        dashCount = 0;
                    }
                }
            }
        }
        if (!isJump)
        {
            if (!isAction) // 액션(공격모션)중이지 않을 때
            {
                // 좌우 이동

                // 앉아 있는 상태가 아닐 때
                if (!isLower)
                {
                    isGuard = false; // 중화가 추가해준 코드
                    if (Input.GetKey(left))
                    {
                        //////// 왼쪽으로 움직일 때 방어 설정
                        if (ctrlType == CtrlType.One)
                        {
                            if (!isturn)
                            {
                                isGuard = true;
                            }
   
                        }
                        if (ctrlType == CtrlType.Two)
                        {
                            if (isturn)
                                isGuard = true;
                        }
                        ///////

                        moveVec3.x = -moveSpeed * Time.deltaTime * dashSpeed; // 이동속도 * 프레임 * 대쉬 속도
                        playerAni.SetBool("IsMove", true);
                        // 움직이고 있는 상태가 아닐 때
                        if (!isMove)
                        {
                            playerAni.SetTrigger("IsMoveFirst");
                            isMove = true;
                        }
                    }
                    else if (Input.GetKey(right))
                    {
                        //////// 오른쪽으로 움직일 때 방어 설정
                        if (ctrlType == CtrlType.One)
                        {
                            if (isturn)
                                isGuard = true;
                        }

                        if (ctrlType == CtrlType.Two)
                        {
                            if (!isturn)
                                isGuard = true;
                        }
                        ///////

                        moveVec3.x = moveSpeed * Time.deltaTime * dashSpeed; // 이동속도 * 프레임 * 대쉬 속도
                        playerAni.SetBool("IsMove", true);
                        // 움직이고 있는 상태가 아닐 때
                        if (!isMove)
                        {
                            playerAni.SetTrigger("IsMoveFirst");
                            isMove = true;
                        }
                    }
                }

                // 점프
                if (Input.GetKeyDown(up) && jumpCount == 0)
                {
                    jumpCount += 1; // 점프 제한 카운트
                    moveVec3 = new Vector3(myRb.velocity.x, 10 * jumpPower, 0); // 기본 값 * 점프력

                    playerAni.SetBool("IsJumpFirst", true);
                    playerAni.SetBool("IsJump", true);

                    // 맨처음 점프 키 눌렀을 때
                    if (!isJump)
                        isJump = true;
                }

                // 하단
                // 앉는 애니메이션 실행
                else if (Input.GetKeyDown(down))
                {
                    isLower = true;
                    playerAni.SetTrigger("IsLowerFirst");
                }
            }
            // 일어나는 애니메이션 실행
            if (Input.GetKeyUp(down))
            {
                if (isLower)
                {
                    isLower = false;
                    if (!isAction)
                        playerAni.SetTrigger("IsLowerLast");
                }
            }
        }


        // 양쪽 방향키를 전부 해제했을 때 이동상태 초기화
        if (!Input.GetKey(right) && Input.GetKeyUp(left) || Input.GetKeyUp(right) && !Input.GetKey(left))
        {
            isMove = false; // 이동 X

            // 연타(대쉬) 체크 시간보다 크면
            if (dashSpeed != 3.5f && !isJump)
                moveVec3 = new Vector2(0, myRb.velocity.y);
            if (dashTimer >= dashTimerLimit)
            {
                isLeft = 0;
                isRight = 0;
            }
            playerAni.SetBool("IsMove", isMove);
        }
        if (!isAction)
            myRb.velocity = (moveVec3);
    }
    protected virtual void PlayerAttack(KeyCode WeekPunch, KeyCode RiverPunch, KeyCode WeekKick, KeyCode RiverKick)
    {
        SaveCommandAttack(WeekPunch, RiverPunch, WeekKick, RiverKick);
        float aniCurrent = playerAni.GetCurrentAnimatorStateInfo(0).normalizedTime;
        bool aniName = false;
        bool noAction = false;
        if (playerAni.GetCurrentAnimatorStateInfo(0).IsName("Char1_Lower") || playerAni.GetCurrentAnimatorStateInfo(0).IsName("Char1_LowerRevease"))
            noAction = true;
        if (playerAni.GetCurrentAnimatorStateInfo(0).IsName("Char1_Punch1_1") || playerAni.GetCurrentAnimatorStateInfo(0).IsName("Char1_Punch1_2"))
            aniName = true;

        if (!isLower && !isJump && !noAction)
        {
            if (Input.GetKeyDown(WeekPunch) && weekPunchCount < 2)
            {
                Debug.Log((aniCurrent - (int)aniCurrent));
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
                //  playerAni.SetTrigger("RiverPunch");
            }
            if (Input.GetKeyDown(WeekKick))
            {
                // playerAni.SetTrigger("WeekKick");
            }
            if (Input.GetKeyDown(RiverKick))
            {
                //  playerAni.SetTrigger("RiverKick");
            }
        }
        else if (isLower)
        {
            if (Input.GetKeyDown(WeekPunch) && weekPunchCount < 2)
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
            if (Input.GetKeyDown(WeekPunch) && weekPunchCount < 2)
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
    public virtual void ReturnCatch()
    {

    }

    /// <summary>
    ///  애니메이션 이벤트
    /// </summary>
    void SetCurrentAction()
    {
        Debug.Log("current");
        currentActionCount += 1;
    }
    void SetPlayerAction()
    {
        Debug.Log(currentActionCount.ToString() + " " + weekPunchCount.ToString());
        if (weekPunchCount == currentActionCount)
        {
            SetDefaultActionState();
        }
        else
            playerAni.SetTrigger("NextPunch");
    }
    void SetDamageState()
    {
        isDamage = false;
        SetDefaultState();
    }
    void SetVelocityY(float _velo)
    {
        myRb.velocity = new Vector2(myRb.velocity.x, _velo);
    }

    void FireAttack()
    {
        FireObject tempFireObj = Instantiate(fireObj, middle.position, Quaternion.identity).GetComponent<FireObject>().SetCtrlType(ctrlType);
        tempFireObj.gameObject.layer = gameObject.layer;
        if ((ctrlType == CtrlType.One && !isturn) || (ctrlType == CtrlType.Two && isturn))
            tempFireObj.FireRight();
        else
            tempFireObj.FireLeft();
        SetDefaultActionState();

    }

    /// <summary>
    /// 기본 상태들
    /// </summary>
    void SetDefaultState()
    {
        if (isMove)
            playerAni.SetTrigger("IsMoveFirst");
        else
            playerAni.SetTrigger("IsIdle");
    }
    public void SetDefaultActionState() // 액션 상태를 처음 상태로 초기화
    {
        Debug.Log("갓태상");
        weekPunchCount = 0;
        currentActionCount = 0;
        playerAni.SetInteger("PunchCount", weekPunchCount);

        isAction = false;
        playerAni.SetBool("IsAction", isAction);
        if (isLower)
            return;
        if (isMove)
            playerAni.SetTrigger("IsMoveFirst");
        else
            playerAni.SetTrigger("IsIdle");
    }

    public void FirstSetting()
    {
        if (ctrlType == CtrlType.One)
            transform.position = new Vector2(-6, -4);
        if (ctrlType == CtrlType.Two)
            transform.position = new Vector2(6, -4);
        Hp = MaxHp;
        weekPunchCount = 0;
        currentActionCount = 0;
        playerAni.SetInteger("PunchCount", weekPunchCount);
        isAction = false;
        isDead = false;
        isGuard = false;
        isJump = false;
        isLower = false;
        isMove = false;
        isDamage = false;
        isturn = false;
        playerAni.SetTrigger("IsIdle");
        playerAni.SetBool("IsAction", isAction);
    }
    /// <summary>
    /// 좌우 회전
    /// </summary>
    public virtual void TurnRight()
    {
        Vector3 turnVec3 = transform.localScale;
        turnVec3.x = -4;
        transform.localScale = turnVec3;
    }
    public virtual void TurnLeft()
    {
        Vector3 turnVec3 = transform.localScale;
        turnVec3.x = 4;
        transform.localScale = turnVec3;
    }

    protected void SetTimer()
    {
        if (commandTimer > commandTimeLimit) // 일정 시간내에 커맨드를 입력 못했을 때
            DeleatCommand(); // 커맨드 제거
        if (!isMove) // 방향 키 누르고 있지 않을 때
            commandTimer += Time.deltaTime;

        if (dashTimer > dashTimerLimit) // 연타 체크 시간이 지났을 때
        {
            dashTimer = 0;
            dashCount = 0;
            isLeft = 0;
            isRight = 0;
        }
        if (dashCount > 0) // 플레이어가 이동 방향키를 눌렀을 때 부터
            dashTimer += Time.deltaTime; // 연타 체크 타이머 작동

    }

    /// <summary>
    ///  변수 정보 반환
    /// </summary>
    /// <param name="coll"></param>
    public float GetArrow()
    {
        float arrow = 1;
        if (!isturn)
            arrow = 1;
        else
            arrow = -1;

        return arrow;
    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Ground")
        {
            if (isJump)
            {

                jumpCount = 0;
                playerAni.SetBool("IsJump", false);
                isJump = false;
                isDamage = false;
                if (isDead)
                    return;
                SetDefaultActionState();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        //DamageCheck(coll);
    }

    public void DamageCheck(Collider2D _coll, string part, int Damage = 100)
    {
        if (isDead)
            return;
        //중화가 추가해준 코드
        if (isGuard)
        {
            Debug.Log("GUARD SUCCEES");
            effectMng.PlayEffect(EffectKind.Guard1, _coll.transform.position);
            return;
        }
        else
            Debug.Log("GUARD FAILED");

        if (ctrlType == CtrlType.One)
            uiMng.p1Info.shake += 0.2f;
        if (ctrlType == CtrlType.Two)
            uiMng.p2Info.shake += 0.2f;
        if (part == "middle")
        {
            //
            if (isJump)
            {
                SetKnockback(16);
            }
            effectMng.PlayEffect(EffectKind.Attack1, _coll.transform.position);
            Debug.Log(111);
            SetHo(-Damage);
            if (!isDead)
                playerAni.SetTrigger("IsDamageMiddle");
            isDamage = true;
            if (isAction)
            {
                SetDefaultActionState();
            }
        }
        else if (part == "top")
        {
            if (isJump)
            {
                SetKnockback(12);
            }
            effectMng.PlayEffect(EffectKind.Attack1, _coll.transform.position);
            Debug.Log(222);
            SetHo(-Damage);
            if (!isDead)
                playerAni.SetTrigger("IsDamageTop");

            isDamage = true;
            if (isAction)
            {
                SetDefaultActionState();
            }
        }
        else if (part == "lower")
        {
            if (isJump)
            {
                SetKnockback(12);
            }
            effectMng.PlayEffect(EffectKind.Attack1, _coll.transform.position);
            Debug.Log(222);
            SetHo(-Damage);
            if (!isDead)
                playerAni.SetTrigger("IsDamageMiddle");

            isDamage = true;
            if (isAction)
            {
                SetDefaultActionState();
            }
        }
    }

    void SetHo(int amount)
    {
        uiMng.SetHp(this, amount);
        if (Hp <= 0)
        {
            isDead = true;
            playerAni.SetTrigger("IsFallDown");
            Time.timeScale = 0.15f;
            SetKnockback(8);
            uiMng.PlayKO(ctrlType);
            gameMng.SetStart(false);
        }

    }
    void SetKnockback(float KnockbackVar) // 넉백 날라가는 정도
    {
        if (ctrlType == CtrlType.One && !isturn)
            myRb.velocity = new Vector2(-KnockbackVar, myRb.velocity.y);
        else if (ctrlType == CtrlType.One && isturn)
            myRb.velocity = new Vector2(KnockbackVar, myRb.velocity.y);
        if (ctrlType == CtrlType.Two && !isturn)
            myRb.velocity = new Vector2(KnockbackVar, myRb.velocity.y);
        else if (ctrlType == CtrlType.One && isturn)
            myRb.velocity = new Vector2(-KnockbackVar, myRb.velocity.y);
    }
    public void SetDamage(int _damage)
    {
        transform.GetChild(3).GetComponent<DamageValue>().SetDamage(_damage);
    }
}
