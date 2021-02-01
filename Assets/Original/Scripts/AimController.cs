using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{
    public Sprite[] sprite;             //照準スプライト
    public Canvas canvas;               //UI

    private Image image;                //照準画像
    private bool change;                //照準の切替フラグ

    // Start is called before the first frame update
    void Start()
    {
        //照準を非表示として初期化
        canvas.enabled = false;

        //照準の切替の初期化
        change = false;

        //照準のImageコンポーネントを取得
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            canvas.enabled = true;      //照準を表示

            var localPos = Vector2.zero;            //照準のlocalPositionを初期化
            var uiCamera = canvas.worldCamera;      //canvasに設定されたカメラを代入

            //canvasのRectTransformコンポーネントを取得
            var canvasRect = canvas.GetComponent<RectTransform>();

            //Zキーが押された時
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //照準の切替がtrueの場合
                if (change)
                {
                    //スプライトの変更
                    //（sprite[0]に格納したスプライトをimageに格納したImageコンポーネントに割り当て）
                    image.sprite = sprite[0];
                    change = false;
                }
                //照準の切替がfalseの場合
                else
                {
                    //スプライトの変更
                    //（sprite[1]に格納したスプライトをimageに格納したImageコンポーネントに割り当て）
                    image.sprite = sprite[1];
                    change = true;
                }
            }
        }
        else
        {
            canvas.enabled = false;     //照準を非表示
        }
    }
}
