using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
    //照準のImageコンポーネント
    [SerializeField]
    private Image aimImage;
    //照準の各色
    [SerializeField]
    private Sprite[] aimSprite;
    //透明度を変更するキャンバス
    [SerializeField]
    private CanvasGroup canvas;
    //透明度が変わるスピードを管理
    [SerializeField]
    private float fadeSpeed = 0.1f;

    public GameManager gameManager;

    private Animator animator;
    private MyStatus myStatus;
    private float alpha;                    //キャンバスの透明度

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myStatus = GetComponent<MyStatus>();

        canvas.alpha = 0f;
        alpha = canvas.alpha;
    }

    // Update is called once per frame
    void Update()
    {
        if (myStatus.Hp == 0 || gameManager.gameStatus != GameManager.GameStatus.Progress)
        {
            canvas.alpha = 0f;

            return;
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            StartFadeIn();

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Reload") || myStatus.EnergyAmount == 0)
            {
                aimImage.sprite = aimSprite[0];
            }
            else
            {
                aimImage.sprite = aimSprite[1];
            }
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
