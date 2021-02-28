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
    //次にHPを減らすまでの時間
    [SerializeField]
    private float nextCountTime = 0f;

    private float maxHpGaugeAmount = 0.85f; //HPゲージの最大割合
    private float hpGaugeAmount;            //HPゲージの割合
    private float countTime = 0f;           //HPを一度減らしてからの経過時間
    private int damage = 0;                 //現在のダメージ量
    private MyController myController;      //プレイヤーの制御スクリプト

    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<MyController>();
        hp = maxHp;
        hpGaugeAmount = maxHpGaugeAmount;
        hpGauge.fillAmount = maxHpGaugeAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //　ダメージなければ何もしない
        if (damage == 0)
        {
            return;
        }
        //　次に減らす時間がきたら
        if (countTime >= nextCountTime)
        {
            //　ダメージ量を10で割った商をHPから減らす
            var tempDamage = damage / 5;
            //　商が0になったら余りを減らす
            if (tempDamage == 0)
            {
                tempDamage = damage % 5;
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
        var tempDamage = opponentAttackPower * 3 - defensePower * 2;

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
        hpGaugeAmount = maxHpGaugeAmount * (float)GetHp() / (float)GetMaxHp();
        hpGauge.fillAmount = hpGaugeAmount;

        if ((float)GetHp() / (float)GetMaxHp() > 0.3f)
        {
            hpIcon.sprite = hpIconSprite[0];
            hpGauge.sprite = hpGaugeSprite[0];
        }
        else
        {
            hpIcon.sprite = hpIconSprite[1];
            hpGauge.sprite = hpGaugeSprite[1];
        }
    }
}
