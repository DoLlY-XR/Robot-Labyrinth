using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    //ビームのパーティクルプレハブ
    public ParticleSystem particlePrefab;
    //銃口
    public Transform muzzle;

    private EnemyController enemyController;
    private EnemyStatus enemyStatus;                      //プレイヤーのステータス管理スクリプト
    private int dTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのAvatorControllerコンポーネントを取得
        enemyController = GetComponent<EnemyController>();
        //プレイヤーのMyStatusコンポーネントを取得
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyController.enemyState == EnemyController.EnemyState.Attack)
        {
            dTime++;
        }
        else
        {
            dTime = 0;
        }

        if (dTime == 100)
        {
            var particleInstance = Instantiate<ParticleSystem>
                    (particlePrefab, muzzle.position, muzzle.rotation);

            BeamAttack beamAttack = particleInstance.GetComponent<BeamAttack>();
            beamAttack.SetAttackPower(enemyStatus.AttackPower);
            Debug.Log("パーティクル生成");

            //パーティクルを開始
            particleInstance.Play(true);
        }
        Debug.Log("dTime = " + dTime);
    }
}
