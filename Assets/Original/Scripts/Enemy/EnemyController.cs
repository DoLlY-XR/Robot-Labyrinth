using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Mob,
        Middle1,
        Middle2,
        Boss
    }
    //敵の状態リスト
    public enum EnemyState
    {
        Idle,
        Walk,
        Chase,
        Attack,
        Freeze,
        Damage,
        Dead
    };

    public EnemyType enemyType;
    //怯みダメージ
    public int falteringDamage = 20;
    //敵の攻撃範囲
    public float[] attackRange = {8f, 12f};
    //敵の追跡範囲
    public float outOfRange = 25f;
    //回転スピード
    public float rotateSpeed = 60f;
    //待ち時間
    public float waitTime = 5f;
    //攻撃後の硬直時間
    public float freezeTimeAfterAttack = 1f;
    //敵の状態
    public EnemyState enemyState;
    //障害物のレイヤー
    public LayerMask obstacleLayer;

    [SerializeField]
    private float distance = 0f;                            //目的地点との距離

    [NonSerialized]
    public Transform playerTransform;                       //プレイヤーのTransformコンポーネント
    //[NonSerialized]
    public EnemyPopUp enemyPop;

    private EnemyStatus enemyStatus;                        //敵のステータス管理スクリプト
    private UnityEngine.AI.NavMeshAgent navMeshAgent;       //エージェント
    private Animator animator;                              //敵のアニメーター
    private Vector3 startPosition;                          //スタート地点
    private Vector3 destination;                            //目的地点
    private float elapsedTime = 0f;                         //Idleの時間
    private int accumulationDamage;                         //蓄積ダメージ
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetState(EnemyState.Idle);
        startPosition = transform.position;
        var randDestination = UnityEngine.Random.insideUnitCircle * 8;
        destination = startPosition + new Vector3(randDestination.x, transform.position.y, randDestination.y);

        if (enemyType == EnemyType.Middle1 || enemyType == EnemyType.Middle2)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (enemyType == EnemyType.Boss)
        {
            this.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStatus == GameManager.GameStatus.Over || gameManager.gameStatus == GameManager.GameStatus.Clear)
        {
            Destroy(gameObject);
        }

        if (enemyState == EnemyState.Dead)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        //見回りまたはキャラクターを追いかける状態
        if (enemyState == EnemyState.Walk || enemyState == EnemyState.Chase)
        {
            //キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (enemyState == EnemyState.Chase)
            {
                SetDestination(playerTransform.position);
                navMeshAgent.SetDestination(destination);
            }
            //animatorの切り替え
            animator.SetFloat("frontSpeed", navMeshAgent.desiredVelocity.magnitude);

            if (enemyState == EnemyState.Walk)
            {
                //目的地に到着したかどうかの判定
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    SetState(EnemyState.Idle);
                    animator.SetFloat("frontSpeed", 0f);
                }
            }
            else if (enemyState == EnemyState.Chase)
            {
                distance = Vector3.Distance(transform.position, playerTransform.position);

                if (distance > outOfRange)
                {
                    playerTransform = null;
                    SetState(EnemyState.Idle);
                }
                else if (distance < attackRange[0])      //攻撃する距離だったら戦闘態勢
                {
                    //硬直時間を過ぎていたら攻撃状態
                    if (elapsedTime > freezeTimeAfterAttack)
                    {
                        SetState(EnemyState.Attack);
                    }   //硬直時間内であれば硬直状態
                    else
                    {
                        SetState(EnemyState.Freeze);
                    }
                }
            }
            //到着していたら一定時間待つ
        }
        else if (enemyState == EnemyState.Idle)
        {
            //待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }   //戦闘態勢の時
        else if (enemyState == EnemyState.Attack || enemyState == EnemyState.Freeze)
        {
            //攻撃状態
            if (enemyState == EnemyState.Attack)
            {
                if (!animator.GetBool("Attack"))
                {
                    SetState(EnemyState.Freeze);
                }
            }   //攻撃後の硬直状態
            else if (enemyState == EnemyState.Freeze)
            {
                //硬直時間を超えたら攻撃
                if (elapsedTime > freezeTimeAfterAttack)
                {
                    SetState(EnemyState.Attack);
                }

                distance = Vector3.Distance(transform.position, playerTransform.position);

                //攻撃する距離から離れたら追跡
                if (distance > attackRange[1])
                {
                    SetState(EnemyState.Chase, playerTransform);
                }
            }

            //プレイヤーの方向を取得
            var playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
            //敵の向きをプレイヤーの方向に少しづつ変える
            var dir = Vector3.RotateTowards(transform.forward, playerDirection, rotateSpeed * Time.deltaTime, 0f);
            //算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (enemyState == EnemyState.Damage)
        {
            //硬直時間を超えたら回復アニメーション
            if (elapsedTime > freezeTimeAfterAttack)
            {
                animator.SetBool("Recover", true);
                SetState(EnemyState.Freeze);
            }
        }
    }

    //敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (enemyState == EnemyState.Dead)
        {
            return;
        }

        enemyState = tempState;

        if (tempState == EnemyState.Walk)
        {
            elapsedTime = 0f;
            var randDestination = UnityEngine.Random.insideUnitCircle * 10;
            destination = startPosition + new Vector3(randDestination.x, transform.position.y, randDestination.y);
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        else if (tempState == EnemyState.Chase)
        {
            animator.SetBool("Attack", false);
            //追いかける対象をセット
            playerTransform = targetObj;
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        }
        else if (tempState == EnemyState.Idle)
        {
            elapsedTime = 0f;
            playerTransform = null;
            animator.SetFloat("frontSpeed", 0f);
            navMeshAgent.isStopped = true;
            distance = 0f;
        }
        else if (tempState == EnemyState.Attack)
        {
            animator.SetFloat("frontSpeed", 0f);
            animator.SetFloat("backSpeed", 0f);
            animator.SetBool("Attack", true);
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
            animator.SetFloat("frontSpeed", 0f);
            animator.SetFloat("backSpeed", 0f);
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Damage)
        {
            elapsedTime = 0f;
            animator.ResetTrigger("Attack");
            animator.SetBool("Damage", true);
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Dead)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            navMeshAgent.isStopped = true;
            enemyPop.PopUpCount--;
            enemyPop.indicateObject.Decrease(this.gameObject);

            if (enemyType == EnemyType.Boss)
            {
                gameManager.gameStatus = GameManager.GameStatus.Clear;
            }
        }
    }

    //敵の状態取得
    public EnemyState GetState()
    {
        return enemyState;
    }

    //目的地の設定
    void SetDestination(Vector3 position)
    {
        elapsedTime += Time.deltaTime;

        //待ち時間を越えたら次の目的地を設定
        if (elapsedTime > waitTime)
        {
            startPosition = transform.position;
            destination = position;
            elapsedTime = 0f;
        }
    }

    //敵のダメージ計算
    public void TakeDamage(float attackedPower, Transform playerTransform)
    {
        if (enemyState != EnemyState.Chase || enemyState != EnemyState.Attack || enemyState != EnemyState.Freeze)
        {
            SetState(EnemyState.Chase, playerTransform);
        }

        accumulationDamage = enemyStatus.SetDamage(attackedPower);

        if (accumulationDamage > falteringDamage)
        {
            accumulationDamage = 0;
            SetState(EnemyState.Damage);
        }
    }
}
