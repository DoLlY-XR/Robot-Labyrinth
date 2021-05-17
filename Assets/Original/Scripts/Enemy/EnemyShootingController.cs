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
    private SoundManager soundManager;
    private int dTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //敵のEnemyControllerコンポーネントを取得
        enemyController = GetComponent<EnemyController>();
        //敵のEnemyStatusコンポーネントを取得
        enemyStatus = GetComponent<EnemyStatus>();
        //敵のSoundManagerコンポーネントを取得
        soundManager = GetComponent<SoundManager>();
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

        if (dTime == 50)
        {
            var particleInstance = Instantiate<ParticleSystem>
                    (particlePrefab, muzzle.position, muzzle.rotation);
            var bulletDirection = new Vector3(enemyController.playerTransform.position.x, enemyController.playerTransform.position.y + 1, enemyController.playerTransform.position.z);
            particleInstance.transform.LookAt(bulletDirection);
            BeamAttack beamAttack = particleInstance.GetComponent<BeamAttack>();
            beamAttack.SetAttackPower(enemyStatus.AttackPower);
            Debug.Log("パーティクル生成");

            //パーティクルを開始
            particleInstance.Play(true);
            soundManager.EnemyShot();
        }
    }
}
