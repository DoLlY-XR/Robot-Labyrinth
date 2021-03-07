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
    //最大エネルギー量
    [SerializeField]
    private int maxEnergyAmount = 10;
    //エネルギー量
    [SerializeField]
    private int energyAmount = 10;
    //エネルギータンク数
    [SerializeField]
    private int energyTankQuantity;
    //高密度エネルギータンク数
    [SerializeField]
    private int highEnergyTankQuantity;
    //次にHPを減らすまでの時間
    [SerializeField]
    private float nextCountTime = 0f;
    //装備パーツの情報
    [SerializeField]
    private EquipmentParts equipmentParts;
    //エネルギータンクの情報
    [SerializeField]
    private ItemInformation energyTank;
    //高密度エネルギータンクの情報
    [SerializeField]
    private ItemInformation highEnergyTank;

    private float countTime = 0f;           //HPを一度減らしてからの経過時間
    private int damage = 0;                 //現在のダメージ量

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        energyTankQuantity = energyTank.Item.quantity;
        highEnergyTankQuantity = highEnergyTank.Item.quantity;

        if (energyTankQuantity == 0 && energyAmount == 0)
        {
            hp -= 10;
            energyTank.ItemQuantity += 10;
        }

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

            if (hp - tempDamage < 0)
            {
                hp = 0;
            }
            else
            {
                hp -= tempDamage;
            }
            damage -= tempDamage;

            countTime = 0f;
        }
        countTime += Time.deltaTime;
    }

    public void SetDamage(float opponentAttackPower)
    {
        var tempDamage = opponentAttackPower * 3 - DefensePower * 2;

        if (tempDamage > 0)
        {
            damage += (int)tempDamage;
        }
        else
        {
            damage += 1;
        }

        countTime = 0f;
    }

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int AttackPower
    {
        get { return attackPower + equipmentParts.WeaponPower; }
    }

    public int DefensePower
    {
        get { return defensePower + equipmentParts.ShieldPower; }
    }

    public int MaxEnergyAmount
    {
        get { return maxEnergyAmount; }
    }

    public int EnergyAmount
    {
        get { return energyAmount; }
        set { energyAmount = value; }
    }

    public int EnergyTankQuantity
    {
        get { return energyTankQuantity; }
        set { energyTankQuantity = value; }
    }

    public int HighEnergyTankQuantity
    {
        get { return highEnergyTankQuantity; }
        set { highEnergyTankQuantity = value; }
    }
}
