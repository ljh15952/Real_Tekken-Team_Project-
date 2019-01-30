using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneScript : MonoBehaviour {

    public List<GameObject> Texts = new List<GameObject>();
    int pivot;
    bool isanimated;


	// Use this for initialization
	void Start () {
        isanimated = false;
        pivot = 2;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isanimated)
            return;
		if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            PressDownArrow();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PressUpArrow();
        }
	}

    void PressUpArrow()
    {
        StartCoroutine(TextUp());

    }

    void PressDownArrow()
    {
        StartCoroutine(TextDown());

    }

    IEnumerator TextUp()
    {
        float Timer = 0.5f;
        isanimated = true;

        Texts[pivot + 2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0);


        while (Timer >= 0)
        {
            Timer -= Time.deltaTime;

            Texts[pivot - 2].transform.localPosition = new Vector3(-2, -115, 0);
            Texts[pivot - 2].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);


            Texts[pivot-1].transform.Translate(new Vector3(0, 1.4f * 2, 0));
            Texts[pivot-1].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);
            Texts[pivot - 1].GetComponent<RectTransform>().localScale -= new Vector3(0.01f, 0, 0);

            Texts[pivot].transform.Translate(new Vector3(0, 1.4f * 2, 0));
            Texts[pivot].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);


            Texts[pivot+1].transform.Translate(new Vector3(0, 1.4f * 2, 0));
            Texts[pivot+1].GetComponent<Text>().color += new Color32(0, 0, 0, 5);

            Texts[pivot + 2].transform.Translate(new Vector3(0, 1.4f * 2, 0));
            Texts[pivot + 2].GetComponent<Text>().color += new Color32(0, 0, 0, 5);
            Texts[pivot + 2].GetComponent<RectTransform>().localScale += new Vector3(0.015f, 0.015f , 0);



            Debug.Log("HI");

            yield return new WaitForEndOfFrame();


           
        }
        Texts[5] = Texts[0];

        for (int i = 0; i < 5; i++)
        {
            Texts[i] = Texts[i + 1];
            Texts[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        isanimated = false;
    }

    IEnumerator TextDown()
    {
        float Timer = 0.5f;
        isanimated = true;

        Texts[pivot - 2].GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0);

        while (Timer >= 0)
        {
            Timer -= Time.deltaTime;

            Texts[pivot + 2].transform.localPosition = new Vector3(0, 51 , 0);
            Texts[pivot + 2].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            Texts[pivot + 1].transform.Translate(new Vector3(0, -1.4f * 2, 0));
            Texts[pivot + 1].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);
            Texts[pivot + 1].GetComponent<RectTransform>().localScale -= new Vector3(0.01f, 0, 0);


            Texts[pivot].transform.Translate(new Vector3(0, -1.4f * 2, 0));
            Texts[pivot].GetComponent<Text>().color -= new Color32(0, 0, 0, 5);

            Texts[pivot - 1].transform.Translate(new Vector3(0, -1.4f * 2, 0));
            Texts[pivot - 1].GetComponent<Text>().color += new Color32(0, 0, 0, 5);

            Texts[pivot - 2].transform.Translate(new Vector3(0, -1.4f * 2, 0));
            Texts[pivot - 2].GetComponent<Text>().color += new Color32(0, 0, 0, 5);
            Texts[pivot - 2].GetComponent<RectTransform>().localScale += new Vector3(0.015f, 0.015f, 0);


            Debug.Log("HI");

            yield return new WaitForEndOfFrame();



        }

        Texts[5] = Texts[4];

        for (int i = 4; i > 0; i--)
        {
            Texts[i] = Texts[i-1];
        }

        for (int i = 0; i < 5; i++)
        {
            Texts[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        Texts[0] = Texts[5];
        isanimated = false;
    }

}
