using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;
using System;
using Google;
using Facebook.Unity;
using System.Threading.Tasks;

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
            Debug.Log("로그인 완료");
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
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Google Login task.IsFaulted");
            }
            else
            {
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
                    UIManager.Instance.googleLogin.googlePannel.SetActive(true);
                    UIManager.Instance.googleLogin.email.text = user.Email;
                    UIManager.Instance.googleLogin.name.text = user.DisplayName;
                    return;
                });
            }
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
            UIManager.Instance.facebookLogin.facebookPannel.SetActive(true);
            UIManager.Instance.facebookLogin.userId.text = userID;
            UIManager.Instance.facebookLogin.name.text = user.DisplayName;
        });
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}