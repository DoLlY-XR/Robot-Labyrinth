using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCameraEffect : MonoBehaviour
{
    //ダメージ演出の時間
    [SerializeField]
    private float effectTime = 1f;
    //ロボットカメラ
    [SerializeField]
    private GameObject robotCamera;
    //レーダーカメラ
    [SerializeField]
    private GameObject radarCamera;
    //照準カメラ
    [SerializeField]
    private GameObject aimCamera;
    //コックピットのGameObject
    [SerializeField]
    private GameObject cookpit;

    private MyStatus myStatus;
    private int tempHp;                         //プレイヤーの仮のHP
    private bool damageEffect = false;
    private float dTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのステータスを取得
        myStatus = GetComponent<MyStatus>();
        //仮のHPを取得
        tempHp = myStatus.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        var robotCameras = robotCamera.GetComponentsInChildren<GlitchFx>();

        if (myStatus.Hp == 0)
        {
            foreach (GlitchFx camera in robotCameras)
            {
                camera.intensity = 1f;
                camera.enabled = true;
            }
            radarCamera.GetComponent<GlitchFx>().intensity = 1f;
            radarCamera.GetComponent<GlitchFx>().enabled = true;
            aimCamera.GetComponent<GlitchFx>().intensity = 1f;
            aimCamera.GetComponent<GlitchFx>().enabled = true;

            return;
        }

        if (tempHp > myStatus.Hp)
        {
            foreach (GlitchFx camera in robotCameras)
            {
                camera.enabled = true;
            }
            radarCamera.GetComponent<GlitchFx>().enabled = true;
            aimCamera.GetComponent<GlitchFx>().enabled = true;

            damageEffect = true;
        }
        else
        {
            if (damageEffect == true)
            {
                dTime += Time.deltaTime;
            }

            if (dTime > effectTime)
            {
                foreach (GlitchFx camera in robotCameras)
                {
                    camera.enabled = false;
                }
                radarCamera.GetComponent<GlitchFx>().enabled = false;
                aimCamera.GetComponent<GlitchFx>().enabled = false;

                damageEffect = false;
                dTime = 0f;
            }
        }

        tempHp = myStatus.Hp;
    }
}
