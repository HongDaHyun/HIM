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
    public GameObject emailPannel;
    public InputField email, password;
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