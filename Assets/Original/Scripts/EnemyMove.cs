using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //移動スピード
    public float walkSpeed = 1f;
    //待ち時間
    public float waitTime = 5f;

    private CharacterController controller;     //敵のコントローラー
    private Animator animator;                  //敵のアニメーター
    private Vector3 startPosition;              //スタート地点
    private Vector3 destination;                //目的地点
    private Vector3 velocity = Vector3.zero;    //速度
    private Vector3 direction;                  //移動方向
    private bool arrived = false;               //到着フラグ
    private float elapsedTime = 0f;             //経過時間

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (!arrived)
        {
            MoveToDestination();
        }
        else
        {
            SetDestination();
        }
    }

    //目的地への移動
    void MoveToDestination()
    {
        if (controller.isGrounded)
        {
            velocity = Vector3.zero;
            animator.SetFloat("speed", 2.0f);
            direction = (destination - transform.position).normalized;
            transform.LookAt(new Vector3(destination.x, transform.position.y, destination.z));
            velocity = direction * walkSpeed;
        }

        if (Vector3.Distance(transform.position, destination) < 0.5f)
        {
            arrived = true;
            animator.SetFloat("speed", 0.0f);
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //目的地の設定
    void SetDestination()
    {
        elapsedTime += Time.deltaTime;

        //　待ち時間を越えたら次の目的地を設定
        if (elapsedTime > waitTime)
        {
            startPosition = transform.position;
            var randDestination = Random.insideUnitCircle * 8;
            destination = startPosition + new Vector3(randDestination.x, transform.position.y, randDestination.y);
            arrived = false;
            elapsedTime = 0f;
        }
    }
}
