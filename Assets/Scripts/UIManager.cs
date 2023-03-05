using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    public EmailLogin emailLogin;
    public GoogleLogin googleLogin;
    public FacebookLogin facebookLogin;
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
public struct GoogleLogin
{
    public GameObject googlePannel;
    public Text email, name;
}

[Serializable]
public struct FacebookLogin
{
    public GameObject facebookPannel;
    public Text userId, name;
}