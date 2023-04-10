using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    public RectTransform wallPaper;
    public LoginPannel loginPannel;
    public GameObject namePannel;
    public EmailLogin emailLogin;
    public ContentPannel contentPannel;
    public PopUpPannel popUpPannel;
    public Pannel selectPannel;

    public int screenWidth, screenHeight;
    public float halfWidth;

    protected override void Awake()
    {
        base.Awake();
        SetSize();
    }

    private void Update()
    {
        if (popUpPannel.pannel.gameObject.activeSelf)
            contentPannel.scrollRect.enabled = false;
        else
            contentPannel.scrollRect.enabled = true;
    }

    public void NamePannelActive()
    {
        loginPannel.btnPannel.SetActive(false);
        loginPannel.startBtn.SetActive(true);
        loginPannel.loginPannel.SetActive(false);
        namePannel.SetActive(true);
    }

    void SetSize()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        halfWidth = screenWidth * 0.5f;
    }

    public void SetPopUp()
    {
        if (selectPannel.pannelState == PannelState.Edit)
            return;
        //Set Position
        BtnManager.Instance.Tab(popUpPannel.pannel.gameObject);

        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;

        popUpPannel.pannel.position = clickPos;

        if (selectPannel.name == "Image")
        {
            for(int i = 0; i < 3; i++)
            {
                popUpPannel.btn[i].gameObject.SetActive(true);
            }
            if (!contentPannel.img_Pannel.img.gameObject.activeSelf)
                popUpPannel.btn[2].gameObject.SetActive(false);
            popUpPannel.mouseImg.transform.SetParent(popUpPannel.btn[0].transform);
        }
        else
        {
            popUpPannel.mouseImg.transform.SetParent(popUpPannel.btn[2].transform);
            popUpPannel.btn[0].gameObject.SetActive(false);
            popUpPannel.btn[1].gameObject.SetActive(false);
            popUpPannel.btn[2].gameObject.SetActive(true);
        }

        if (popUpPannel.pannel.anchoredPosition.x > halfWidth)
        {
            popUpPannel.pannel.pivot = new Vector2(1, 1);
            popUpPannel.mouseImg.anchoredPosition = new Vector2(260, 20);
        }
        else
        {
            popUpPannel.pannel.pivot = new Vector2(0, 1);
            popUpPannel.mouseImg.anchoredPosition = new Vector2(-230, 20);
        }
    }
}

[Serializable]
public struct LoginPannel
{
    public GameObject loginPannel, loginBtn, btnPannel, startBtn;
}

[Serializable]
public struct EmailLogin
{
    public GameObject emailPannel, emailCreatePannel;
    public InputField email, password;
    public InputField c_email, c_password, c_passwordCheck, c_code;
    public string code;
}

[Serializable]
public struct ContentPannel
{
    public ScrollRect scrollRect;
    public RectTransform pannel;
    public ImgPannel img_Pannel;
    public ProfilePannel profile_Pannel;
}

[Serializable]
public struct ImgPannel
{
    public Pannel pannel;
    public RawImage img;
}

[Serializable]
public struct ProfilePannel
{
    public Birth birth;
}

[Serializable]
public struct Birth
{
    public Text yearTxt, monthTxt, dayTxt;
}

[Serializable]
public struct PopUpPannel
{
    public RectTransform pannel;
    public RectTransform mouseImg;
    public Button[] btn;
    public Text[] txt;
}