using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Pannel_Image : Pannel
{
    public RawImage img;

    public void OnClickImgLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);

            //용량 제한 (50000000Byte -> 50MB)
            if (selected.Length > 50000000)
            {
                img.gameObject.SetActive(false);
                return;
            }
            if (string.IsNullOrEmpty(file)) //파일 없으면 리턴
            {
                img.gameObject.SetActive(false);
                return;
            }
            img.gameObject.SetActive(true);
            StartCoroutine(LoadImg(file));
        });
    }

    IEnumerator LoadImg(string path)
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

        img.texture = tex;
        //img.SetNativeSize();
        //ImgSizeSet();
    }

    public override void EditMode()
    {
        base.EditMode();
    }

    //void ImgSizeSet()
    //{
    //    Vector2 imgSize = img.rectTransform.sizeDelta;

    //    if (imgSize.x < contentRect.sizeDelta.x)
    //    {
    //        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentRect.sizeDelta.x);
    //        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgSize.y * (contentRect.sizeDelta.x / imgSize.x));
    //    }
    //    else if (imgSize.y < contentRect.sizeDelta.y)
    //    {
    //        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentRect.sizeDelta.y);
    //        img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgSize.x * (contentRect.sizeDelta.y / imgSize.y));
    //    }
    //}
}
