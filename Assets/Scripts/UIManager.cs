using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class UIManager : Singleton<UIManager>
{
    public LoginPannel loginPannel;
    public GameObject namePannel;
    public EmailLogin emailLogin;
    public ContentPannel contentPannel;

    public void NamePannelActive()
    {
        loginPannel.btnPannel.SetActive(false);
        loginPannel.startBtn.SetActive(true);
        loginPannel.loginPannel.SetActive(false);
        namePannel.SetActive(true);
    }

    public IEnumerator LoadImg(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/Image";

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllBytes(savePath + fileName + ".png", fileData);

        byte[] temp = File.ReadAllBytes(savePath + fileName + ".png");

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(temp);

        contentPannel.imgPannel.img.texture = tex;
        contentPannel.imgPannel.img.SetNativeSize();
        ImgSizeSet(contentPannel.imgPannel.img, contentPannel.imgPannel.contents.rect.size);
    }

    void ImgSizeSet(RawImage img, Vector2 vec)
    {
        float imgX = img.rectTransform.sizeDelta.x;
        float imgY = img.rectTransform.sizeDelta.y;

        if(imgX < vec.x)
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vec.x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (vec.x / imgX));
        }
        else if(imgY < vec.y)
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vec.y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (vec.y / imgY));
        }
    }

    public IEnumerator ScaleUp(RectTransform rect,float maxScale)
    {
        while(rect.transform.localScale.x < maxScale)
        {
            rect.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForEndOfFrame();
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
    public ImgPannel imgPannel;
}

[Serializable]
public struct ImgPannel
{
    public RectTransform contents;
    public RawImage img;
}