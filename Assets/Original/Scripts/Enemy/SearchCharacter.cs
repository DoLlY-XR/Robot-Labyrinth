using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    private EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        //　プレイヤーキャラクターを発見
        if (col.tag == "Player")
        {
            //　敵キャラクターの状態を取得
            EnemyController.EnemyState enemyStatus = enemy.GetState();
            //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if (enemyStatus == EnemyController.EnemyState.Idle || enemyStatus == EnemyController.EnemyState.Walk)
            {
                enemy.SetState(EnemyController.EnemyState.Chase, col.transform);
            }
        }
    }
}
