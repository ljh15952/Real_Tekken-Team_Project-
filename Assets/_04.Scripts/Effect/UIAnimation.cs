using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIAnimation : MonoBehaviour {

    // Use this for initialization

    public void PlayFadeInOut()
    {
        StartCoroutine("FadeInOut");
    }
    IEnumerator FadeInOut()
    {
        GetComponent<Image>().DOFade(1, 1);
        yield return new WaitForSeconds(1);
        GameMng.Instance.GameReset();
        GetComponent<Image>().DOFade(0, 1);
        yield return new WaitForSeconds(1);
        GameMng.Instance.uiMng.PlayRoundStart();
    }
}
