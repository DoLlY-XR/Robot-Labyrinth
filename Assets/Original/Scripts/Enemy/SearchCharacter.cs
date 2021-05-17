using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private float searchAngle = 70f;

    private EnemyController enemy;
    private float hideTime = 0f;

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

            //　主人公の方向
            var playerDirection = col.transform.root.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);

            Debug.DrawLine(transform.position + Vector3.up, col.transform.position + Vector3.up, Color.blue);

            //　サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
                if ((enemyStatus == EnemyController.EnemyState.Idle || enemyStatus == EnemyController.EnemyState.Walk) &&
                    !Physics.Linecast(transform.position + Vector3.up, col.transform.position + Vector3.up, obstacleLayer))
                {
                    Debug.Log("プレイヤー発見: " + angle);
                    enemy.SetState(EnemyController.EnemyState.Chase, col.transform.root);
                }

                //　プレイヤーが障害物で見えない時
                if (Physics.Linecast(transform.position + Vector3.up, col.transform.position + Vector3.up, obstacleLayer))
                {
                    hideTime += Time.deltaTime;

                    if (hideTime > 7f)
                    {
                        enemy.SetState(EnemyController.EnemyState.Idle);
                        hideTime = 0f;
                    }
                }
            }
        }
    }
}
