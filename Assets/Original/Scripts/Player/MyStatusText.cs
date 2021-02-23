using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatusText : MonoBehaviour
{
    //ステータスを表示するテキスト
    [SerializeField]
    private Text text;

    private MyStatus myStatus;              //プレイヤーのステータス管理スクリプト
    private int maxHp;                      //最大HP
    private int hp;                         //HP
    private int attackPower;                //攻撃力
    private int defensePower;               //防御力
    
    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのMyStatusコンポーネントを取得
        myStatus = GetComponent<MyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        //myStatusからステータスを更新
        maxHp = myStatus.GetMaxHp();
        hp = myStatus.GetHp();
        attackPower = myStatus.GetAttackPower();
        defensePower = myStatus.GetDefensePower();

        //ステータスtextを更新
        text.text = "HP:\t\t\t" + hp + " / " + maxHp + "\n" + "攻撃力:\t" + attackPower + "\n" + "防御力:\t" + defensePower + "\n\n" + "武器:\t\t銃\n";
    }
}
