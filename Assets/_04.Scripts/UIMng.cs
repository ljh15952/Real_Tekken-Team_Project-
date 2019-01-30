using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using TMPro;

[System.Serializable]
public class PlayerInfo
{
    public Image Hp;
    public Image Hp2;
    public Image HpBack;
    public Transform mom;
    public Vector3 originPos;

    public Image[] roundWin;
    public float shake;
    public float hpTimer;
}

public class UIMng : MonoBehaviour {

    public GameMng gameMng;

    public Sprite[] hpStateSprite;

    public PlayerInfo p1Info;
    public PlayerInfo p2Info;

    public float hpTimerLimit;

    public TextMeshProUGUI startText;
    public Text Count;
    public Text FirstWin;
    public Text SecondWin;
    public Text Equal;

    public TextMeshProUGUI K;
    public TextMeshProUGUI O;

    public Image fadeImage;

    bool isDoing = true;
    bool end;

    public int nCount=99;
    public float shakeAmount;

    private void Update()
    {
        PlayHpTimer(p1Info);
        PlayHpTimer(p2Info);
        HpShake(p1Info);
        HpShake(p2Info);
        if (nCount==0)
        {
            Debug.Log("00000");
            if(p1Info.Hp.fillAmount > p2Info.Hp.fillAmount)
                FirstWin.gameObject.SetActive(true);
            if(p1Info.Hp.fillAmount < p2Info.Hp.fillAmount)
                SecondWin.gameObject.SetActive(true);
            if (p1Info.Hp.fillAmount == p2Info.Hp.fillAmount)
                Equal.gameObject.SetActive(true);
            end = true;
        }
        if(p1Info.Hp.fillAmount<=0)
        {
            end = true;

        }
        if(p2Info.Hp.fillAmount<=0)
        {
            end = true;
        }


    }

    void PlayHpTimer(PlayerInfo _info)
    {
        if (_info.hpTimer >= hpTimerLimit)
        {
            _info.Hp2.fillAmount -= Time.deltaTime;
            if (_info.Hp2.fillAmount <= _info.Hp.fillAmount)
            {
                _info.Hp2.fillAmount = _info.Hp.fillAmount;
                _info.hpTimer = 0;
            }
        }
        if (_info.Hp2.fillAmount > _info.Hp.fillAmount)
            _info.hpTimer += Time.deltaTime;
    }
    public void SetHp(Player player, int amount)
    {
        player.Hp += amount;
        if(player.ctrlType == CtrlType.One)
        {
            p1Info.Hp.fillAmount = player.Hp / player.MaxHp;
            p1Info.hpTimer = 0;
            if (p1Info.Hp.fillAmount <= 0.3f)
            {
                p1Info.Hp.sprite = hpStateSprite[1];
                p1Info.HpBack.sprite = hpStateSprite[3];
            }
        }
        if (player.ctrlType == CtrlType.Two)
        {
            p2Info.Hp.fillAmount = player.Hp / player.MaxHp;
            p2Info.hpTimer = 0;
            if (p2Info.Hp.fillAmount <= 0.3f)
            {
                p2Info.Hp.sprite = hpStateSprite[1];
                p2Info.HpBack.sprite = hpStateSprite[3];
            }
        }
    }
    public void SetHp(Player_Photon player, int amount)
    {
        player.Hp += amount;
        if (player.ctrlType == CtrlType.One)
        {
            p1Info.Hp.fillAmount = player.Hp / player.MaxHp;
            p1Info.hpTimer = 0;
            if (p1Info.Hp.fillAmount <= 0.3f)
            {
                p1Info.Hp.sprite = hpStateSprite[1];
                p1Info.HpBack.sprite = hpStateSprite[3];
            }
        }
        if (player.ctrlType == CtrlType.Two)
        {
            p2Info.Hp.fillAmount = player.Hp / player.MaxHp;
            p2Info.hpTimer = 0;
            if (p2Info.Hp.fillAmount <= 0.3f)
            {
                p2Info.Hp.sprite = hpStateSprite[1];
                p2Info.HpBack.sprite = hpStateSprite[3];
            }
        }
    }

    public void PlayRoundStart()
    {
        StartCoroutine("RoundStart");
    }
    IEnumerator RoundStart()
    {
        gameMng.nCount = 99;
        p1Info.Hp.sprite = hpStateSprite[0];
        p1Info.HpBack.sprite = hpStateSprite[2];
        p2Info.Hp.sprite = hpStateSprite[0];
        p2Info.HpBack.sprite = hpStateSprite[2];
        StartCoroutine("FadeInHPbar");
        startText.DOFade(0, 0);
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_OutlineSoftness, 0f, 0f);
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_FaceDilate, -1f, 0);

        startText.text = "Round " + gameMng.GetRound();
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_FaceDilate, 0.6f, 1);
        startText.DOFade(1, 1);

        Debug.Log(ShaderUtilities.ID_GlowPower);
        yield return new WaitForSeconds(0.8f);
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_LightAngle, 0, 0);

        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_LightAngle, 6, 1.5f);

        yield return new WaitForSeconds(1.0f);
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_OutlineSoftness, -1f, 0.5f);
        PlayChangeTo(startText.materialForRendering, ShaderUtilities.ID_FaceDilate, 0.3f,0.25f);

        startText.DOFade(0, 0);
        startText.text = "<i>Fight!!!!</i>";
        startText.GetComponent<RectTransform>().DOScale(10, 0);
        startText.GetComponent<RectTransform>().DOScale(1, 0.25f);
        startText.DOFade(1, 0.25f);

        yield return new WaitForSeconds(0.5f);
        //
        startText.DOFade(0, 0.25f);
        gameMng.SetStart(true);
        gameMng.SetPlayerActive(true);
    }
    IEnumerator FadeInHPbar()
    {
        Count.DOFade(0, 0);
        for(int i = 0; i < 3; i++)
        {
            p1Info.roundWin[i].DOFade(0, 0);
            p2Info.roundWin[i].DOFade(0, 0);
        }
        p1Info.Hp.DOFillAmount(0, 0);
        p1Info.Hp2.DOFillAmount(0, 0);
        p2Info.Hp.DOFillAmount(0, 0);
        p2Info.Hp2.DOFillAmount(0, 0);

        p1Info.Hp.DOFade(0, 0);
        p2Info.Hp.DOFade(0, 0);

        p1Info.HpBack.DOFade(0, 0);
        p2Info.HpBack.DOFade(0, 0);

        p1Info.HpBack.DOFade(1, 1);
        p2Info.HpBack.DOFade(1, 1);

        yield return new WaitForSeconds(1);
        Count.DOFade(1, 1);
        p1Info.Hp.DOFade(1, 1);
        p2Info.Hp.DOFade(1, 1);
        p1Info.Hp2.DOFade(1, 1);
        p2Info.Hp2.DOFade(1, 1);
        p1Info.Hp.DOFillAmount(1, 1.5f);
        p1Info.Hp2.DOFillAmount(1, 1.5f);
        p2Info.Hp.DOFillAmount(1, 1.5f);
        p2Info.Hp2.DOFillAmount(1, 1.5f);
        for (int i = 0; i < 3; i++)
        {
            p1Info.roundWin[i].DOFade(1, 1);
            p2Info.roundWin[i].DOFade(1, 1);
            yield return new WaitForSeconds(0.1f);

        }
    }
    IEnumerator FadeOutHPbar()
    {
        Count.DOFade(0, 1 * Time.timeScale);
        for (int i = 0; i < 3; i++)
        {
            p1Info.roundWin[i].DOFade(0, 1 * Time.timeScale);
            p2Info.roundWin[i].DOFade(0, 1 * Time.timeScale);
            yield return new WaitForSeconds(0.1f * Time.timeScale);

        }
        p1Info.Hp.DOFillAmount(0, 1 * Time.timeScale);
        p1Info.Hp2.DOFade(0, 1 * Time.timeScale);
        p2Info.Hp.DOFade(0, 1 * Time.timeScale);
        p2Info.Hp2.DOFade(0, 1 * Time.timeScale);
        yield return new WaitForSeconds(1 * Time.timeScale);
        p1Info.HpBack.DOFade(0, 1 * Time.timeScale);
        p2Info.HpBack.DOFade(0, 1 * Time.timeScale);

    }
    public void PlayKO(CtrlType losePlayer)
    {
        StartCoroutine("KO");
        switch (losePlayer)
        {
            case CtrlType.One:
                p2Info.roundWin[gameMng.p2WinCount].DOColor(new Color(255, 0, 0, 0), 1);
                gameMng.p2WinCount += 1;
                break;
            case CtrlType.Two:
                p1Info.roundWin[gameMng.p1WinCount].DOColor(new Color(255, 0, 0, 0), 1);
                gameMng.p1WinCount += 1;
                break;

        }
    }

    IEnumerator KO()
    {
        gameMng.SetRound(1);
        K.DOFade(1, 0);
        O.DOFade(1, 0);
        O.rectTransform.DOScale(0, 0);
        K.rectTransform.DOScale(0, 0);
        StartCoroutine("FadeOutHPbar");

        yield return new WaitForSeconds(0.4f);
        K.rectTransform.DOScale(3, 0);
        K.rectTransform.DOScale(1, 0.05f);
        yield return new WaitForSeconds(0.1f);
        O.rectTransform.DOScale(3, 0);
        O.rectTransform.DOScale(1, 0.05f);
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 1;

        K.DOFade(0, 0.5f);
        O.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        fadeImage.GetComponent<UIAnimation>().PlayFadeInOut();
    }
    public void TimeText(float _time)
    {
        Count.text = _time.ToString("N0");
    }
    public void HpShake(PlayerInfo info)
    {
        if(info.shake > 0)
        {
            info.mom.localPosition = p1Info.originPos + Random.insideUnitSphere * shakeAmount;
            info.shake -= Time.deltaTime;
        }
        else if(p1Info.shake <0)
        {
            info.shake = 0;
            info.mom.localPosition = info.originPos;
        }
    }


    void PlayChangeTo(Material _material, int nameID, float _to, float _time)
    {
        StartCoroutine(ChangeTo(_material, nameID, _to, _time));
    }

    IEnumerator ChangeTo(Material _material, int nameID, float _to, float _time = 1.0f) // 값이 바뀐다.
    {
        float tempDis = 0;
        float _value = _material.GetFloat(nameID);
        float _timer = 0;
      //  if (_value >= _to)
         //   tempDis = _value - _to;
        //else
            tempDis = _to - _value;
        while(true)
        { 
            _value += tempDis / (_time / Time.deltaTime );
            _material.SetFloat(nameID, _value);
            _timer += Time.deltaTime;
            if (_timer >= _time)
                break;
            yield return new WaitForEndOfFrame();
        }
        _value = _to;
        _material.SetFloat(nameID, _value);

    }
}
