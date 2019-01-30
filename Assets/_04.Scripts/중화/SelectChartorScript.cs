using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SelectChartorScript : MonoBehaviour {

    //470

    public GameObject P1;
    public GameObject P2;

    public Image[] Chartors = new Image[4];

    int P1Ct;
    int P2Ct;

    bool P1_isReady;
    bool P2_isReady;

	void Start () {
        P1_isReady = false;
        P1_isReady = false;
        P1Ct = 1;
        P2Ct = 2;
        Chartors[P1Ct - 1].color = new Color32(255, 255, 255, 255);
        Chartors[P2Ct - 1].color = new Color32(255, 255, 255, 255);
	}
	
	void Update ()
    {
        Select_ON(P1Ct);
        Select_ON(P2Ct);

        InputKey_P1();
        InputKey_P2();


        PickChartor();
    }

    void PickChartor()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            P1_isReady = true;
            //P1 Pick Chartor
            //1플레이어의 케릭터 정보를 넘겨줌
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            P2_isReady = true;
            //P2 Pick Chartor
            //2플레이어의 케릭터 정보르 넘겨줌
        }

        if(P1_isReady&&P2_isReady)
        {
            Debug.Log("LOADGAMESCENE");
            StartCoroutine(Delay());
            // 씬 넘겨 게임씬으로 
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene2");

    }


    void Select_ON(int num)
    {
        Chartors[num - 1].color = new Color32(255, 255, 255, 255);
    }

    void Select_OFF(int num)
    {
        Chartors[num - 1].color = new Color32(100, 100, 100, 255);
    }

    void InputKey_P1()
    {
        if (P1_isReady)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (P1Ct == 1)
            {
                P1.transform.Translate(new Vector3(470 * 3, 0, 0));

                Select_OFF(P1Ct);
                P1Ct = 4;
                Select_ON(P1Ct);

                return;
            }
            P1.transform.Translate(new Vector3(-470, 0, 0));

            Select_OFF(P1Ct);
            P1Ct--;
            Select_ON(P1Ct);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (P1Ct == 4)
            {
                P1.transform.Translate(new Vector3(-470 * 3, 0, 0));

                Select_OFF(P1Ct);
                P1Ct = 1;
                Select_ON(P1Ct);


                return;
            }
            P1.transform.Translate(new Vector3(470, 0, 0));

            Select_OFF(P1Ct);
            P1Ct++;
            Select_ON(P1Ct);

        }
    }
    void InputKey_P2()
    {
        if (P2_isReady)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (P2Ct == 1)
            {
                P2.transform.Translate(new Vector3(470 * 3, 0, 0));

                Select_OFF(P2Ct);
                P2Ct = 4;
                Select_ON(P2Ct);


                return;
            }
            P2.transform.Translate(new Vector3(-470, 0, 0));

            Select_OFF(P2Ct);
            P2Ct--;
            Select_ON(P2Ct);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (P2Ct == 4)
            {
                P2.transform.Translate(new Vector3(-470 * 3, 0, 0));

                Select_OFF(P2Ct);
                P2Ct = 1;
                Select_ON(P2Ct);

                return;
            }
            P2.transform.Translate(new Vector3(470, 0, 0));

            Select_OFF(P2Ct);
            P2Ct++;
            Select_ON(P2Ct);

        }

    }
}
