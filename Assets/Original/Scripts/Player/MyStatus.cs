using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyStatus : MonoBehaviour
{
    //MaxHP
    [SerializeField]
    private int maxHp = 100;
    //HP
    [SerializeField]
    private int hp;
    //攻撃力
    [SerializeField]
    private int attackPower = 10;
    //防御力
    [SerializeField]
    private int defensePower = 10;
    //HPゲージ
    [SerializeField]
    private Image hPIcon;
    //HPアイコンのスプライト
    [SerializeField]
    private Sprite[] hPIconSprite;
    //HPゲージ
    [SerializeField]
    private Image hPGauge;
    //HPゲージのスプライト
    [SerializeField]
    private Sprite[] hPGaugeSprite;
    //次にHPを減らすまでの時間
    [SerializeField]
    private float nextCountTime = 0f;

    private float maxHPGaugeAmount = 0.85f; //HPゲージの最大割合
    private float hPGaugeAmount;            //HPゲージの割合
    private float countTime = 0f;           //HPを一度減らしてからの経過時間
    private int damage = 0;                 //現在のダメージ量
    private MyController myController;      //プレイヤーの制御スクリプト

    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<MyController>();
        hp = maxHp;
        hPGaugeAmount = maxHPGaugeAmount;
        hPGauge.fillAmount = maxHPGaugeAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //　ダメージなければ何もしない
        /*if (damage == 0)
        {
            return;
        }*/
        //　次に減らす時間がきたら
        if (countTime >= nextCountTime)
        {
            //　ダメージ量を10で割った商をHPから減らす
            var tempDamage = damage / 10;
            //　商が0になったら余りを減らす
            if (tempDamage == 0)
            {
                tempDamage = damage % 10;
            }
            hp -= tempDamage;
            damage -= tempDamage;

            countTime = 0f;
        }

        UpdateHPValue();
        countTime += Time.deltaTime;
    }

    public void SetDamage(int opponentAttackPower)
    {
        var tempDamage = opponentAttackPower * 5 - defensePower * 2;

        if (tempDamage > 0)
        {
            damage += tempDamage;
        }
        else
        {
            damage += 1;
        }

        countTime = 0f;
    }

    public int GetHp()
    {
        return hp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefensePower()
    {
        return defensePower;
    }

    public void UpdateHPValue()
    {
        hPGaugeAmount = maxHPGaugeAmount * (float)GetHp() / (float)GetMaxHp();
        hPGauge.fillAmount = hPGaugeAmount;

        if ((float)GetHp() / (float)GetMaxHp() > 0.3f)
        {
            hPIcon.sprite = hPIconSprite[0];
            hPGauge.sprite = hPGaugeSprite[0];
        }
        else
        {
            hPIcon.sprite = hPIconSprite[1];
            hPGauge.sprite = hPGaugeSprite[1];
        }
    }
}
