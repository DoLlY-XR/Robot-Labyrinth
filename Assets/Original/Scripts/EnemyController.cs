using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //敵の状態リスト
    public enum EnemyState
    {
        Idle,
        Walk,
        Chase,
        Attack,
        Freeze,
        Adjust,
        Damage,
        Dead
    };

    //プレイヤーのTransformコンポーネント
    public Transform playerTransform;
    //敵の攻撃範囲
    public float attackRange = 5f;
    //回転スピード
    public float rotateSpeed = 45f;
    //位置調整のスピード
    public float adjustSpeed = 5f;
    //待ち時間
    public float waitTime = 5f;
    //攻撃後の硬直時間
    public float freezeTimeAfterAttack = 5f;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;          //エージェント
    private EnemyState enemyStatus;             //敵の状態
    private Animator animator;                  //敵のアニメーター
    private Vector3 startPosition;              //スタート地点
    private Vector3 destination;                //目的地点
    private Vector3 direction;                  //移動方向
    private float elapsedTime = 0f;             //Idleの時間
    private float distance = 0f;                //目的地点との距離

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetState(EnemyState.Idle);
        startPosition = transform.position;
        var randDestination = Random.insideUnitCircle * 8;
        destination = startPosition + new Vector3(randDestination.x, transform.position.y, randDestination.y);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        elapsedTime += Time.deltaTime;

        if (enemyStatus == EnemyState.Dead)
        {
            return;
        }
        //見回りまたはキャラクターを追いかける状態
        if (enemyStatus == EnemyState.Walk || enemyStatus == EnemyState.Chase)
        {
            //キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (enemyStatus == EnemyState.Chase)
            {
                SetDestination(playerTransform.position);
                navMeshAgent.SetDestination(destination);
            }
            //animatorの切り替え
            animator.SetFloat("frontSpeed", navMeshAgent.desiredVelocity.magnitude);

            if (enemyStatus == EnemyState.Walk)
            {
                //目的地に到着したかどうかの判定
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    SetState(EnemyState.Idle);
                    animator.SetFloat("frontSpeed", 0f);
                }
            }
            else if (enemyStatus == EnemyState.Chase)
            {
                //攻撃する距離だったら戦闘態勢
                if (distance < attackRange)
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
        else if (enemyStatus == EnemyState.Idle)
        {
            //待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }   //戦闘態勢の時
        else if (enemyStatus == EnemyState.Attack || enemyStatus == EnemyState.Freeze || enemyStatus == EnemyState.Adjust)
        {
            //攻撃状態
            if (enemyStatus == EnemyState.Attack)
            {
                if (!animator.GetBool("Attack"))
                {
                    SetState(EnemyState.Freeze);
                }
            }   //攻撃後の硬直状態
            else if (enemyStatus == EnemyState.Freeze)
            {
                //硬直時間を超えたら攻撃
                if (elapsedTime > freezeTimeAfterAttack)
                {
                    SetState(EnemyState.Attack);
                }

                //攻撃する距離から離れたら追跡
                if (distance > attackRange + 10f)
                {
                    SetState(EnemyState.Chase, playerTransform);
                }   //目標に近すぎると位置調整
                else if (distance < attackRange - 10f)
                {
                    SetState(EnemyState.Adjust);
                }
            }   //調整状態
            else if (enemyStatus == EnemyState.Adjust)
            {
                transform.position -= transform.forward * adjustSpeed * Time.deltaTime;

                //攻撃範囲に戻ると攻撃
                if (distance > attackRange)
                {
                    SetState(EnemyState.Attack);
                }
            }

            //プレイヤーの方向を取得
            var playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
            //敵の向きをプレイヤーの方向に少しづつ変える
            var dir = Vector3.RotateTowards(transform.forward, playerDirection, rotateSpeed * Time.deltaTime, 0f);
            //算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    //敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (enemyStatus == EnemyState.Dead)
        {
            return;
        }

        enemyStatus = tempState;

        if (tempState == EnemyState.Walk)
        {
            elapsedTime = 0f;
            var randDestination = Random.insideUnitCircle * 10;
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
            animator.SetFloat("frontSpeed", 0f);
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Attack)
        {
            animator.SetFloat("frontSpeed", 0f);
            animator.SetFloat("backSpeed", 0f);
            animator.SetBool("Attack", true);
            navMeshAgent.velocity = Vector3.zero;
            //audioSource.PlayOneShot(attackSound);
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
            animator.SetFloat("frontSpeed", 0f);
            animator.SetFloat("backSpeed", 0f);
        }
        else if (tempState == EnemyState.Adjust)
        {
            animator.SetFloat("frontSpeed", 0f);
            animator.SetFloat("backSpeed", 2f);
        }
        else if (tempState == EnemyState.Damage)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Damage");
            navMeshAgent.isStopped = true;
        }
        else if (tempState == EnemyState.Dead)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
            navMeshAgent.isStopped = true;
        }
    }

    //敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return enemyStatus;
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
}
