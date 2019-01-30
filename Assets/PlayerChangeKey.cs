using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
public class PlayerChangeKey : MonoBehaviour
{
    public Text p1Up;
    public Text p1Down;
    public Text p1Left;
    public Text p1Right;
    public Text p1WeakPunch;
    public Text p1PowerPunch;
    public Text p1WeakKick;
    public Text p1PowerKick;
    public Text p2Up;
    public Text p2Down;
    public Text p2Left;
    public Text p2Right;
    public Text p2WeakPunch;
    public Text p2PowerPunch;
    public Text p2WeakKick;
    public Text p2PowerKick;

    int currentKey;
    KeyCode currentKeycode;
    string kcodeString;
    // Use this for initialization
    void Start()
    {

        p1Up.text = PlayerPrefs.GetString("p1Up", "W");

        p1Down.text = PlayerPrefs.GetString("p1Down", "S");

        p1Left.text = PlayerPrefs.GetString("p1Left", "A");

        p1Right.text = PlayerPrefs.GetString("p1Right", "D");

        p1WeakPunch.text = PlayerPrefs.GetString("p1WeakPunch", "J");


        p1PowerPunch.text = PlayerPrefs.GetString("p1PowerPunch", "K");


        p1WeakKick.text = PlayerPrefs.GetString("p1WeakKick", "U");


        p1PowerKick.text = PlayerPrefs.GetString("p1PowerKick", "I");


        p2Up.text = PlayerPrefs.GetString("p2Up", KeyCode.Keypad8.ToString());


        p2Down.text = PlayerPrefs.GetString("p2Down", KeyCode.Keypad5.ToString());


        p2Left.text = PlayerPrefs.GetString("p2Left", KeyCode.Keypad4.ToString());


        p2Right.text = PlayerPrefs.GetString("p2Right", KeyCode.Keypad6.ToString());


        p2WeakPunch.text = PlayerPrefs.GetString("p2WeakPunch", KeyCode.L.ToString());


        p2PowerPunch.text = PlayerPrefs.GetString("p2PowerPunch", KeyCode.Semicolon.ToString());

        p2WeakKick.text = PlayerPrefs.GetString("p2WeakKick", KeyCode.Period.ToString());

        p2PowerKick.text = PlayerPrefs.GetString("p2PowerKick", KeyCode.Slash.ToString());

    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetKey();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

    }
    public void SetCurrentkey(int _key) // 1
    {
        currentKey = _key;
    }
    void SetKey()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKey(kcode))
                {
                    currentKeycode = kcode;
                    break;
                }
            }
        }
        kcodeString = currentKeycode.ToString();
        if (currentKey == 1)
        {
            PlayerPrefs.SetString("p1Up", kcodeString);
            p1Up.text = kcodeString;
        }
        if (currentKey == 2)
        {
            PlayerPrefs.SetString("p1Down", kcodeString);
            p1Down.text = kcodeString;
        }
        if (currentKey == 3)
        {
            PlayerPrefs.SetString("p1Left", kcodeString);
            p1Left.text = kcodeString;
        }
        if (currentKey == 4)
        {
            PlayerPrefs.SetString("p1Right", kcodeString);
            p1Right.text = kcodeString;
        }
        if (currentKey == 5)
        {
            PlayerPrefs.SetString("p1WeakPunch", kcodeString);
            p1WeakPunch.text = kcodeString;
        }
        if (currentKey == 6)
        {
            PlayerPrefs.SetString("p1PowerPunch", kcodeString);
            p1PowerPunch.text = kcodeString;
        }
        if (currentKey == 7)
        {
            PlayerPrefs.SetString("p1WeakKick", kcodeString);
            p1WeakKick.text = kcodeString;
        }
        if (currentKey == 8)
        {
            PlayerPrefs.SetString("p1PowerKick", kcodeString);
            p1PowerKick.text = kcodeString;
        }
        if (currentKey == 9)
        {
            PlayerPrefs.SetString("p2Up", kcodeString);
            p2Up.text = kcodeString;
        }
        if (currentKey == 10)
        {
            PlayerPrefs.SetString("p2Down", kcodeString);
            p2Down.text = kcodeString;
        }
        if (currentKey == 11)
        {
            PlayerPrefs.SetString("p2Left", kcodeString);
            p2Left.text = kcodeString;
        }
        if (currentKey == 12)
        {
            PlayerPrefs.SetString("p2Right", kcodeString);
            p2Right.text = kcodeString;
        }
        if (currentKey == 13)
        {
            PlayerPrefs.SetString(" p2WeakPunch", kcodeString);
            p2WeakPunch.text = kcodeString;
        }
        if (currentKey == 14)
        {
            PlayerPrefs.SetString("p2PowerPunch", kcodeString);
            p2PowerPunch.text = kcodeString;
        }
        if (currentKey == 15)
        {
            PlayerPrefs.SetString("p2WeakKick", kcodeString);
            p2WeakKick.text = kcodeString;
        }
        if (currentKey == 16)
        {
            PlayerPrefs.SetString("p2PowerKick", kcodeString);
            p2PowerKick.text = kcodeString;
        }
        currentKey = 999;
    }
}
