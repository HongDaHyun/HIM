using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnManager : Singleton<BtnManager>
{
    #region Email Login
    public void EmailPannel()
    {
        UIManager.Instance.emailLogin.emailPannel.SetActive(true);
    }

    public void EmailCreateAccountBtn()
    {
        FirebaseAuthManager.Instance.EmailCreateAccount(UIManager.Instance.emailLogin.email.text, UIManager.Instance.emailLogin.password.text);
    }

    public void EmailLoginBtn()
    {
        FirebaseAuthManager.Instance.EmailLogin(UIManager.Instance.emailLogin.email.text, UIManager.Instance.emailLogin.password.text);
    }
    #endregion

    public void GoogleLoginBtn()
    {
        FirebaseAuthManager.Instance.GoogleLogin();
    }

    public void FacebookLoginBtn()
    {
        FirebaseAuthManager.Instance.FacebookLogin();
    }

    public void LogoutBtn()
    {
        FirebaseAuthManager.Instance.Logout();
    }
}
