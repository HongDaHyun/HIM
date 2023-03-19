using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    public LoginPannel loginPannel;
    public GameObject namePannel;
    public EmailLogin emailLogin;

    public void NamePannelActive()
    {
        loginPannel.btnPannel.SetActive(false);
        loginPannel.startBtn.SetActive(true);
        loginPannel.loginPannel.SetActive(false);
        namePannel.SetActive(true);
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