using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
    //プレイヤー
    [SerializeField]
    private GameObject player;
    //ステータスを表示するテキスト
    [SerializeField]
    private Text statusText;
    //HPゲージ
    [SerializeField]
    private Image hpIcon;
    //HPアイコンのスプライト
    [SerializeField]
    private Sprite[] hpIconSprite;
    //HPゲージ
    [SerializeField]
    private Image hpGauge;
    //HPゲージのスプライト
    [SerializeField]
    private Sprite[] hpGaugeSprite;
    //エネルギーゲージ
    [SerializeField]
    private Slider energySlider;
    //エネルギータンク数を表示するテキスト
    [SerializeField]
    private Text energyTankQuantity;
    //高密度エネルギータンク数を表示するイメージ
    [SerializeField]
    private Image[] highEnergyTankQuantity;

    private MyStatus myStatus;              //プレイヤーのステータス管理スクリプト
    private int maxHp;                      //最大HP
    private int hp;                         //HP
    private int attackPower;                //攻撃力
    private int defensePower;               //防御力
    private float maxHpGaugeAmount = 0.84f; //HPゲージの最大割合
    private float hpGaugeAmount;            //HPゲージの割合

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのMyStatusコンポーネントを取得
        myStatus = player.GetComponent<MyStatus>();
        //myStatusからmaxHpを取得
        maxHp = myStatus.MaxHp;
        //hpをmaxHpとして初期化
        hp = maxHp;
        //HPUIの数値を初期化
        hpGaugeAmount = maxHpGaugeAmount;
        hpGauge.fillAmount = maxHpGaugeAmount;
        //エネルギーゲージの初期化
        energySlider.value = (float)myStatus.EnergyAmount / (float)myStatus.MaxEnergyAmount;
        //エネルギータンク数の初期化
        energyTankQuantity.text = "× " + myStatus.EnergyTankQuantity;
        //高密度エネルギータンク数の初期化
        for (int i = 0; i < highEnergyTankQuantity.Length; i++)
        {
            if (myStatus.HighEnergyTankQuantity > i)
            {
                highEnergyTankQuantity[i].transform.Find("Panel").GetComponent<Image>().color = new Color(1f, 0.7647059f, 0.2862745f, 1f);
            }
            else
            {
                highEnergyTankQuantity[i].transform.Find("Panel").GetComponent<Image>().color = new Color(1f, 0.7647059f, 0.2862745f, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //myStatusからステータスを更新
        hp = myStatus.Hp;
        attackPower = myStatus.AttackPower;
        defensePower = myStatus.DefensePower;

        //ステータスtextを更新
        statusText.text = "HP:\t\t\t" + hp + " / " + maxHp + "\n" + "攻撃力:\t" + attackPower + "\n" + "防御力:\t" + defensePower + "\n\n" + "武器:\t\t銃\n";

        //HPUIの数値を更新
        hpGaugeAmount = maxHpGaugeAmount * (float)hp / (float)maxHp;
        hpGauge.fillAmount = hpGaugeAmount;
        if ((float)hp / (float)maxHp > 0.3f)
        {
            hpIcon.sprite = hpIconSprite[0];
            hpGauge.sprite = hpGaugeSprite[0];
        }
        else
        {
            hpIcon.sprite = hpIconSprite[1];
            hpGauge.sprite = hpGaugeSprite[1];
        }

        //エネルギーゲージの更新
        energySlider.value = (float)myStatus.EnergyAmount / (float)myStatus.MaxEnergyAmount;

        //エネルギータンク数の更新
        energyTankQuantity.text = "× " + myStatus.EnergyTankQuantity;

        //高密度エネルギータンク数の更新
        for (int i = 0; i < highEnergyTankQuantity.Length; i++)
        {
            if (myStatus.HighEnergyTankQuantity > i)
            {
                highEnergyTankQuantity[i].transform.Find("Panel").GetComponent<Image>().color = new Color(1f, 0.7647059f, 0.2862745f, 1f);
            }
            else
            {
                highEnergyTankQuantity[i].transform.Find("Panel").GetComponent<Image>().color = new Color(1f, 0.7647059f, 0.2862745f, 0f);
            }
        }
    }
}
