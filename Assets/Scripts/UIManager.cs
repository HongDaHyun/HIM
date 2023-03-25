using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class UIManager : Singleton<UIManager>
{
    public RectTransform wallPaper;
    public LoginPannel loginPannel;
    public GameObject namePannel;
    public EmailLogin emailLogin;
    public ContentPannel contentPannel;

    public int screenWidth, screenHeight;

    protected override void Awake()
    {
        base.Awake();
        SetSize();
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
    public Pannel img_Pannel;
}