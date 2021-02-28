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

    private DamageRoomEffect damageRoomEffect;
    private bool damageEffect = false;
    private float dTime = 0f;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //コックピットのDamageRoomEffectを取得
        damageRoomEffect = cookpit.GetComponent<DamageRoomEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        var robotCameras = robotCamera.GetComponentsInChildren<GlitchFx>();

        if (damageRoomEffect.damageCameraEffect)
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
    }
}
