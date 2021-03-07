using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
    //透明度を変更するキャンバス
    [SerializeField]
    private CanvasGroup canvas;
    //透明度が変わるスピードを管理
    [SerializeField]
    private float fadeSpeed = 0.1f;

    private MyStatus myStatus;
    private float alpha;                    //キャンバスの透明度

    // Start is called before the first frame update
    void Start()
    {
        myStatus = GetComponent<MyStatus>();

        canvas.alpha = 0f;
        alpha = canvas.alpha;
    }

    // Update is called once per frame
    void Update()
    {
        if (myStatus.Hp == 0)
        {
            canvas.alpha = 0f;

            return;
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            StartFadeIn();
        }
        else
        {
            StartFadeOut();
        }
        canvas.alpha = alpha;
    }

    //FadeInを開始
    void StartFadeIn()
    {
        alpha += fadeSpeed;
        if (alpha >= 1f)
        {
            alpha = 1f;
        }
    }

    //FadeOutを開始
    void StartFadeOut()
    {
        alpha -= fadeSpeed;
        if (alpha <= 0f)
        {
            alpha = 0f;
        }
    }
}
