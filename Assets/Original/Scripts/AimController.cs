using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
    public float fadeSpeed = 0.02f;         //透明度が変わるスピードを管理

    private float red, green, blue, alfa;   //パネルの色、不透明度を管理
    private bool aimSwitch = false;            //照準の表示を管理するフラグ
    private RawImage image;                    //透明度を変更するパネルのイメージ

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
        red = image.color.r;
        green = image.color.g;
        blue = image.color.b;
        alfa = image.color.a;
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            StartFadeOut();
        }
        else
        {
            StartFadeIn();
        }
    }

    //FadeInを開始
    void StartFadeIn()
    {
        alfa -= fadeSpeed;
        SetAlpha();
        if (alfa <= 0f)
        {
            image.enabled = false;
            alfa = 0f;
        }
    }

    //FadeOutを開始
    void StartFadeOut()
    {
        image.enabled = true;
        alfa += fadeSpeed;
        SetAlpha();
        if (alfa >= 1f)
        {
            alfa = 1f;
        }
    }

    //Alpha値を設定
    void SetAlpha()
    {
        image.color = new Color(red, green, blue, alfa);
    }
}
