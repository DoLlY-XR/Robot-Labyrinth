using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject partsPrefab;

    private EnemyController enemyController;
    private EnemyStatus enemyStatus;
    private ItemHold player;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyController.enemyState == EnemyController.EnemyState.Dead)
        {
            count++;
            player = enemyController.playerTransform.GetComponent<ItemHold>();

            if (count == 1)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                var random = Random.Range(0, 100);
                Debug.Log("random = " + random);

                Random.InitState(System.DateTime.Now.Millisecond);
                var random2 = Random.Range(0, 10);
                int plus = 0;
                if (random2 < 6)
                {
                    plus = 0;
                }
                else if (random2 < 9)
                {
                    plus = 1;
                }
                else if (random2 < 10)
                {
                    plus = 2;
                }
                Debug.Log("random2 = " + random2);

                if (enemyController.enemyType == EnemyController.EnemyType.Middle1)
                {
                    random = 16;
                }
                else if (enemyController.enemyType == EnemyController.EnemyType.Middle2)
                {
                    random = 18;
                }

                if (random % 20 < 5)        //エネルギータンク
                {
                    player.AddItem(ItemInformation.ItemType.EnergyTank);
                }
                else if (random % 20 < 8)   //高密度エネルギータンク
                {
                    player.AddItem(ItemInformation.ItemType.HighEnergyTank);
                }
                else if (random % 20 < 13)  //修復パーツ(小)
                {
                    player.AddItem(ItemInformation.ItemType.SmallRepairParts);
                }
                else if (random % 20 < 15)  //修復パーツ(中)
                {
                    player.AddItem(ItemInformation.ItemType.MediumRepairParts);
                }
                else if (random % 20 < 16)  //修復パーツ(大)
                {
                    player.AddItem(ItemInformation.ItemType.HighRepairParts);
                }
                else if (random % 20 < 18)  //火力パーツ
                {
                    var sendParts = partsPrefab;
                    sendParts.GetComponent<PartsInformation>().Parts = new PartsInformation.PartsInfo(PartsInformation.PartsType.AttackParts, enemyStatus.Lv + plus);

                    player.AddParts(sendParts);
                }
                else if (random % 20 < 20)  //頑強パーツ
                {
                    var sendParts = partsPrefab;
                    sendParts.GetComponent<PartsInformation>().Parts = new PartsInformation.PartsInfo(PartsInformation.PartsType.ShieldParts, enemyStatus.Lv + plus);

                    player.AddParts(sendParts);
                }
            }
        }
    }
}

/*
 * アイテム一覧
 * ・エネルギータンク        25%  5
 * ・高密度エネルギータンク  15%  8
 * ・修復パーツ(小)          25%  13
 * ・修復パーツ(中)          10%  15
 * ・修復パーツ(大)          5%   16
 * ・火力パーツ              10%  18
 * ・頑強パーツ              10%  20
 * 
 * ステータスパーツ
 * ・+0                     60%  6
 * ・+1                     30%  9
 * ・+2                     10%  10
 */