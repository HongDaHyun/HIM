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

    public void EmailCreatePannel()
    {
        UIManager.Instance.emailLogin.emailCreatePannel.SetActive(true);
        UIManager.Instance.emailLogin.c_email.enabled = true;
        UIManager.Instance.emailLogin.c_password.enabled = true;
        UIManager.Instance.emailLogin.c_passwordCheck.enabled = true;
    }

    public void EmailSenderBtn()
    {
        string c_email = UIManager.Instance.emailLogin.c_email.text;
        string c_password = UIManager.Instance.emailLogin.c_password.text;
        string c_passwordCheck = UIManager.Instance.emailLogin.c_passwordCheck.text;
        if (c_email == "" || c_password == "" || c_passwordCheck == "" || c_password != c_passwordCheck)
        {
            return;
        }

        FirebaseAuthManager.Instance.EmailSender(UIManager.Instance.emailLogin.c_email.text);
        UIManager.Instance.emailLogin.c_email.enabled = false;
        UIManager.Instance.emailLogin.c_password.enabled = false;
        UIManager.Instance.emailLogin.c_passwordCheck.enabled = false;
    }

    public void EmailCreateAccountBtn()
    {
        if (UIManager.Instance.emailLogin.c_code.text != UIManager.Instance.emailLogin.code || UIManager.Instance.emailLogin.c_code.text == "")
            return;
        FirebaseAuthManager.Instance.EmailCreateAccount(UIManager.Instance.emailLogin.c_email.text, UIManager.Instance.emailLogin.c_password.text);
        UIManager.Instance.emailLogin.emailCreatePannel.SetActive(false);
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

    public void Tab(GameObject Object)
    {
        if(!Object.activeSelf)
            Object.SetActive(true);
        else
            Object.SetActive(false);
    }
}
