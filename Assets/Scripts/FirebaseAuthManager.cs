using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Google;
using Facebook.Unity;

public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
{
    private FirebaseAuth auth; //로그인과 회원가입 사용
    private FirebaseUser user; //인증 완료된 유저 정보
    private Credential credential;

    public string userID => user.UserId; //유저 아이디

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Logout();
        }
        if (GoogleSignIn.Configuration == null) //구글
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                WebClientId = "868082087905-bhem7om5v3pnulob7fcitc0n8v7e9570.apps.googleusercontent.com"
            };
        }
        if (!FB.IsInitialized) //페이스북
            FB.Init(InitCallback, OnHideUnity);
        else
            FB.ActivateApp();

        auth.StateChanged += OnChanged; //이벤트 핸들러 (계정 상태가 바뀔 때마다 호출)
    }

    #region 페이스북 Init
    private void InitCallback() //초기화
    {
        if (FB.IsInitialized)
            FB.ActivateApp();
        else
            Debug.LogError("Failed to Initialize the Facebook SDK");
    }

    private void OnHideUnity(bool isGameShown) //로그인 과정 시간 멈추는 역할
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion

    private void OnChanged(object sender, EventArgs e)
    {
        if(auth.CurrentUser != user) // 동일하지 않다 -> 계정 상태 변화
        {
            bool signed = auth.CurrentUser != null; //현재 유저가 null도 아니다
            if(!signed && user != null)
            {
                Debug.Log("로그아웃");
            }
            user = auth.CurrentUser;
            if(signed)
            {
                Debug.Log("로그인");
            }
                
        }
    }

    private string RandomCode(int length)
    {
        string codeChars = "0123456789ABCDEFGHIJKLNMOPQRSTUVWXYZ";

        System.Text.StringBuilder sb = new System.Text.StringBuilder(length);
        System.Random r = new System.Random();

        for(int i = 0; i < length; i++)
        {
            int pos = r.Next(codeChars.Length);
            char c = codeChars[pos];
            sb.Append(c);
        }
        return sb.ToString();
    }

    public void EmailSender(string toEmail)
    {
        string code = RandomCode(6);
        UIManager.Instance.emailLogin.code = code;
        string fromMail = "hdh36115@gmail.com";
        string fromPassword = "jbypyawxmsxfhpuy";

        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromMail);
        message.Subject = "HI'M(하이아임) 인증 코드";
        message.To.Add(new MailAddress(toEmail));
        message.Body = code; //"<html><body> test </body></html>";
        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, fromPassword),
            EnableSsl = true,
        };

        smtpClient.Send(message);
    }

    public void EmailCreateAccount(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if(task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if(task.IsFaulted)
            {
                //회원가입 실패 이유 -> 이메일 비정상/ 비밀번호 간단/ 이미 가입된 이메일 등...
                Debug.LogError("회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log("회원가입 완료");
        });
    }

    public void EmailLogin(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //로그인 실패 이유 -> 이메일 비정상/ 비밀번호 간단/ 이미 가입된 이메일 등...
                Debug.LogError("로그인 실패");
                return;
            }
            FirebaseUser newUser = task.Result;
        });
    }

    public void GoogleLogin()
    {
        if (GoogleSignIn.Configuration == null)
        {
            // 설정
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                WebClientId = "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"
            };
        }

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google Login task.IsCanceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Google Login task.IsFaulted");
                return;
            }
            credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    signInCompleted.SetCanceled();
                    Debug.Log("Google Login authTask.IsCanceled");
                    return;
                }
                if (authTask.IsFaulted)
                {
                    signInCompleted.SetException(authTask.Exception);
                    Debug.Log("Google Login authTask.IsFaulted");
                    return;
                }

                user = authTask.Result;
                UIManager.Instance.NamePannelActive();
            });
        });
    }

    public void FacebookLogin()
    {
        List<string> perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, FacebookCallback);
    }

    private void FacebookCallback(ILoginResult result)
    {
        if (result.Error != null)
            Debug.LogError(string.Format("Auth Error : {0}", result.Error));
        else
        {
            if (FB.IsLoggedIn)
            {
                AccessToken aToken = AccessToken.CurrentAccessToken;

                foreach (string perm in aToken.Permissions)
                    //Debug.Log(string.Format("Permissions : {0}", perm));

                StartCoroutine(FBLogin(aToken));
            }
            else
                Debug.LogError("User Login Canceled");
        }
    }

    IEnumerator FBLogin(AccessToken aToken)
    {
        while (string.IsNullOrEmpty(aToken.TokenString))
            yield return null;

        credential = FacebookAuthProvider.GetCredential(aToken.TokenString);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Facebook Login task.isCanceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Facebook Login task.IsFaulted : " + task.Exception);
                return;
            }

            user = task.Result;
            UIManager.Instance.NamePannelActive();
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}