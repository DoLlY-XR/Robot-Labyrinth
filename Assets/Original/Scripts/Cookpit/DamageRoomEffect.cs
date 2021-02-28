using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageRoomEffect : MonoBehaviour
{
    //コックピットの部屋
    [SerializeField]
    private GameObject controllRoom;
    //プレイヤーのGameObject
    [SerializeField]
    private GameObject player;

    [NonSerialized]
    public bool damageCameraEffect = false;     //カメラのダメージ演出のフラグ

    private Color originalRoomColor;            //controllRoomマテリアルの元の色
    private MyStatus myStatus;                  //プレイヤーのMyStatus
    private int tempHp;                         //プレイヤーの仮のHP
    private bool damageRoomEffect = false;       //部屋のダメージ演出のフラグ

    // Start is called before the first frame update
    void Start()
    {
        //controllRoomマテリアルの元の色を取得
        originalRoomColor = controllRoom.transform.GetChild(0).GetComponent<Renderer>().material.color;
        //プレイヤーのMystatusを取得
        myStatus = player.GetComponent<MyStatus>();
        //仮のHPを取得
        tempHp = myStatus.GetHp();
    }

    // Update is called once per frame
    void Update()
    {
        //controllRoomの全ての子オブジェクトが持つRendererコンポーネントの参照の配列を取得
        var roomMaterials = controllRoom.GetComponentsInChildren<Renderer>();

        if (tempHp > myStatus.GetHp())
        {
            foreach (Renderer childMaterial in roomMaterials)
            {
                childMaterial.material.color = new Color(0.5f, 0f, 0f, 0.5f);
            }

            damageRoomEffect = true;
            damageCameraEffect = true;
        }
        else
        {
            if (damageRoomEffect == true)
            {
                foreach (Renderer childMaterial in roomMaterials)
                {
                    childMaterial.material.color = Color.Lerp(childMaterial.material.color, originalRoomColor, Time.deltaTime);
                }

                if (controllRoom.transform.GetChild(0).GetComponent<Renderer>().material.color == originalRoomColor)
                {
                    damageRoomEffect = false;
                }
            }
            damageCameraEffect = false;
        }
        
        tempHp = myStatus.GetHp();
    }
}
