using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour
{
    public ColorGradingModel.Settings a;
    public PostProcessingProfile backProfile;
    public PostProcessingProfile allProfile;

    public Camera backCamera;
    public Camera playerCamera;
    float zoomHeight;
    float zoomwidth;
    public Transform camTransform;

    // How long the object should shake for.
    public float shake = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public bool isFixing;
    public bool isAction;
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }
    private void Update()
    {
        if (Mathf.Abs(GameMng.Instance.Hero1.transform.position.x - GameMng.Instance.Hero2.transform.position.x) > 16)
            isFixing = true;
        else if(!isAction)
        {
            Debug.Log("origin");
            Vector3 tempVelo = new Vector3();
            Vector3 CameraPos = (GameMng.Instance.Hero1.transform.position + GameMng.Instance.Hero2.transform.position) / 2;
            CameraPos.y = 0;
            CameraPos.x = Mathf.Clamp(CameraPos.x, 0, 23);
            transform.position = Vector3.SmoothDamp(transform.position, CameraPos, ref tempVelo, 0.02f, 50);
            originalPos = CameraPos;
            isFixing = false;
        }
        if (Input.GetKeyDown(KeyCode.Z))
            ZoomCharacter(false, CtrlType.One);
        if (Input.GetKeyDown(KeyCode.X))
            ZoomCharacter(false, CtrlType.Two);
        if (Input.GetKeyDown(KeyCode.C))
            SetDeFaultState();
        if (Input.GetKeyDown(KeyCode.V))
            SetSaturation(0, 0.5f);
        if (Input.GetKeyDown(KeyCode.Q))
            shake += 0.5f;
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        }
        else if (shake < 0)
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void ZoomCharacter(bool isTurn, CtrlType ctrlType)
    {
      //  StartCoroutine(DoRedChange( 2, 0.25f));

        backCamera.DOOrthoSize(3, 0.5f);
        playerCamera.DOOrthoSize(3, 0.5f);

        zoomHeight = 3;
        zoomwidth = 0;
        if (ctrlType == CtrlType.One)
        {
            if (!isTurn)
            {
                transform.DOMove(GameMng.Instance.Hero1.transform.position + new Vector3 (zoomwidth, zoomHeight, 0), 0.5f);
            }
            else
            {
                transform.DOMove(GameMng.Instance.Hero1.transform.position + new Vector3(-zoomwidth, zoomHeight, 0), 0.5f);
            }
        }
        else if (ctrlType == CtrlType.Two)
        {
            if (!isTurn)
            {
                transform.DOMove(GameMng.Instance.Hero2.transform.position + new Vector3(-zoomwidth, zoomHeight, 0), 0.5f);
            }
            else
            {
                transform.DOMove(GameMng.Instance.Hero2.transform.position + new Vector3(zoomwidth, zoomHeight, 0), 0.5f);
            }
        }
    }
    public void SetDeFaultState()
    {
        StartCoroutine(DoRedChange( 0, 0));
        StartCoroutine(DoBlackAndWhite(1, 0.25f));

        backCamera.DOOrthoSize(5, 0.5f);
        playerCamera.DOOrthoSize(5, 0.5f);
        transform.DOMove(originalPos, 0.5f);
    }
    public void SetDeFaultState(float grayScale)
    {
        StartCoroutine(DoRedChange(0, 0.25f));
        StartCoroutine(DoEyeAdaptation(5, 0.25f));
        StartCoroutine(DoBlackAndWhite(grayScale, 0.25f));

        backCamera.DOOrthoSize(5, 0.5f);
        playerCamera.DOOrthoSize(5, 0.5f);
        transform.DOMove(originalPos, 0.5f);
    }
    public void SetSaturation(float endValue, float time)
    {
        StartCoroutine(DoBlackAndWhite(0, 0.5f));
    }
    IEnumerator DoRedChange(float endValue, float time)
    {
        float changeValue = backProfile.colorGrading.settings.channelMixer.red.y;
        Debug.Log(changeValue);
        Debug.Log(endValue);

        while (changeValue != endValue)
        {
            changeValue = Mathf.MoveTowards(changeValue, endValue, 
                (changeValue > endValue ? changeValue - endValue : endValue - changeValue) / (time * 60));
            a = backProfile.colorGrading.settings;
            a.channelMixer.red.y = changeValue;
            backProfile.colorGrading.settings = a;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator DoBlackAndWhite(float endValue, float time)
    {
        float changeValue = backProfile.colorGrading.settings.basic.saturation;
        while (changeValue != endValue)
        {
            changeValue = Mathf.MoveTowards(changeValue, endValue,
                (changeValue > endValue ? changeValue - endValue : endValue - changeValue) / (time * 60));
            a = backProfile.colorGrading.settings;
            a.basic.saturation = changeValue;
            backProfile.colorGrading.settings = a;
            yield return new WaitForEndOfFrame();
        }
    }
    public void SetEyeAdaptation(float endValue, float time)
    {
        StartCoroutine(DoEyeAdaptation(endValue, time));
    }
    IEnumerator DoEyeAdaptation(float endValue, float time)
    {
        float changeValue = allProfile.eyeAdaptation.settings.highPercent;
        while (changeValue != endValue)
        {
            changeValue = Mathf.MoveTowards(changeValue, endValue, 
                (changeValue > endValue ? changeValue - endValue : endValue - changeValue) / (time * 60));

            EyeAdaptationModel.Settings tempSetting = allProfile.eyeAdaptation.settings;
            tempSetting.highPercent = changeValue;
            allProfile.eyeAdaptation.settings = tempSetting;
            yield return new WaitForEndOfFrame();
        }
    }
}
