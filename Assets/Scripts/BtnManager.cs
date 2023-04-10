using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnManager : Singleton<BtnManager>
{
    #region Email Login

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

    public void NextTab(GameObject obj)
    {
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        obj.SetActive(true);
    }

    public void SelectPannel(Pannel pannel)
    {
        UIManager.Instance.selectPannel = pannel;
        UIManager.Instance.SetPopUp();
    }

    public void EditBtn()
    {
        UIManager.Instance.selectPannel.EditMode();
    }

    public void SetLastSibling(RectTransform rect)
    {
        rect.SetAsLastSibling();
    }

    public void NextDateBtn(string name)
    {
        switch(name)
        {
            case "Year":
                GameManager.Instance.curDate = GameManager.Instance.curDate.AddYears(1);
                break;
            case "Month":
                GameManager.Instance.curDate = GameManager.Instance.curDate.AddMonths(1);
                break;
            case "Day":
                GameManager.Instance.curDate = GameManager.Instance.curDate.AddDays(1);
                break;
        }
    }

    public void UpdateDateTxt()
    {
        UIManager.Instance.contentPannel.profile_Pannel.birth.yearTxt.text = GameManager.Instance.curDate.Year.ToString() + "³â";
        UIManager.Instance.contentPannel.profile_Pannel.birth.monthTxt.text = GameManager.Instance.curDate.Month.ToString("00") + "¿ù";
        UIManager.Instance.contentPannel.profile_Pannel.birth.dayTxt.text = GameManager.Instance.curDate.Day.ToString("00") + "ÀÏ";
    }
}
