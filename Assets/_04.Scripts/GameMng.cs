using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public static GameMng Instance;
    public CameraController camCon;
    public UIMng uiMng;
    public Player Hero1; // Right
    public Player Hero2; // Left

    public int p1WinCount;
    public int p2WinCount;

    public Transform[] walls;

    int round;
    bool isStart;

    public float nCount = 99;
    bool isWall;
    private void Awake()
    {
        if (Hero1.name == "Fox")
            Hero1 = Hero1.GetComponent<Fox>();
        if (Hero2.name == "Fox")
            Hero2 = Hero2.GetComponent<Fox>();
        round = 1;
        SetPlayerActive(false);
        Instance = this;
        uiMng.PlayRoundStart();
    }

    private void Update()
    {
        if (isStart)
        {
            if (camCon.isFixing && !isWall)
            {
                Vector3 wallVec3 = new Vector3();
                if (!Hero1.isturn)
                {
                    wallVec3 = Hero1.transform.position;
                    wallVec3.y = 3;
                    wallVec3.x = Hero1.transform.position.x - 1.7f;
                    walls[0].position = wallVec3;
                }
                else if (Hero1.isturn)
                {
                     wallVec3 = Hero1.transform.position;
                    wallVec3.y = 3;
                    wallVec3.x = Hero1.transform.position.x + 1.7f;
                    walls[0].position = wallVec3;
                }
                if (Hero2.isturn)
                {
                     wallVec3 = Hero2.transform.position;
                    wallVec3.y = 3;
                    wallVec3.x = Hero2.transform.position.x - 1.7f;
                    walls[1].position = wallVec3;
                }
                else if (!Hero2.isturn)
                {
                     wallVec3 = Hero2.transform.position;
                    wallVec3.y = 3;
                    wallVec3.x = Hero2.transform.position.x + 1.7f;
                    walls[1].position = wallVec3;
                }
                isWall = true;
            }
            else if (!camCon.isFixing && isWall)
            {
                walls[0].position = new Vector3(1000, 1000);
                walls[1].position = new Vector3(1000, 1000);
                isWall = false;
            }
            if (Hero1.transform.position.x < Hero2.transform.position.x)
            {
                Hero1.TurnRight();
                Hero2.TurnLeft();
                Hero1.isturn = false; //중화가 추가
                Hero2.isturn = false; //중화가 추가

            }
            else
            {
                Hero1.TurnLeft();
                Hero2.TurnRight();
                Hero1.isturn = true; //중화가 추가
                Hero2.isturn = true;//중화가 추가
            }
            nCount -= Time.deltaTime;
            uiMng.TimeText(nCount);
        }
    }

    public void SetPlayerActive(bool _value)
    {
        Hero1.enabled = _value;
        Hero2.enabled = _value;
    }
    public int GetRound()
    {
        return round;
    }
    public void SetRound(int value)
    {
        round += value;
    }

    public void SetStart(bool _value)
    {
        isStart = _value;
    }
    public bool GetStart()
    {
        return isStart;
    }
    public void GameReset() // 라운드 종료시 호출
    {
        Hero1.FirstSetting();
        Hero2.FirstSetting();
    }
}
