using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyController : MonoBehaviour
{
    public Transform neckTemp;                      //首のデフォルトの位置
    public Transform Neck;                          //首の位置
    public Transform spineTemp;                     //脊椎のデフォルトの位置
    public Transform Spine;                         //脊椎の位置
    public Transform weaponBone;                    //武器の位置
    public float speed = 6.0F;                      //歩行速度
    public float stepSpeed = 10.0F;                 //ステップ速度
    public float stepTime = 3.0F;                   //ステップ時間
    public float jumpSpeed = 8.0F;                  //ジャンプ力
    public float gravity = 20.0F;                   //重力の大きさ
    public float rotateSpeed = 3.0F;                //回転速度
    public float postureRotateLimit = 30f;          //銃撃姿勢で回転出来る限度

    [NonSerialized]
    public Animator animator;                       //プレイヤーのアニメーター
    [NonSerialized]
    public float tempRotateY = 0.0f;                //ShootingControlスクリプトで使うrotateY
    [NonSerialized]
    public float rotateY = 0.0f;                    //y軸回転量
    [NonSerialized]
    public Vector2 stickL;                          //左手のアナログスティック
    [NonSerialized]
    public Vector2 stickR;                          //右手のアナログスティック
    [NonSerialized]
    public bool flagY = true;                       //他のスクリプトで使うフラグY

    private CharacterController controller;         //プレイヤーのコントローラー
    private MyStatus myStatus;                      //プレイヤーのステータス管理スクリプト
    private Vector3 moveDirection = Vector3.zero;   //プレイヤーの移動量
    private float runSpeed;                         //現在の歩行速度
    private bool combatPosture = false;             //戦闘態勢の切替フラグ
    private bool isSteping = false;                 //ステップフラグ
    private float dTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのCharacterControllerコンポーネントを取得
        controller = GetComponent<CharacterController>();
        //プレイヤーのAnimatorコンポーネントを取得
        animator = GetComponent<Animator>();
        //プレイヤーのMyStatusコンポーネントを取得
        myStatus = GetComponent<MyStatus>();
        //デフォルトの歩行速度を初期化
        runSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        stickL = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);      // 左手のアナログスティックの向きを取得
        stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);      // 右手のアナログスティックの向きを取得

        //プレイヤーの接地判定
        if (controller.isGrounded)
        {
            SwitchingCombatPosture();

            if (!animator.GetBool("is_setting"))        //非戦闘アニメーション時
            {
                Normal();
            }
            else if (animator.GetBool("is_setting"))    //戦闘アニメーション時
            {
                Combat();
            }
        }

        //プレイヤーの座標移動
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    protected virtual void LateUpdate()
    {
        rotateY += stickR.y;
        tempRotateY = rotateY;

        if (rotateY > postureRotateLimit)
        {
            rotateY = postureRotateLimit;
            flagY = false;
        }
        else if (rotateY < -postureRotateLimit)
        {
            rotateY = -postureRotateLimit;
            flagY = false;
        }
        else
        {
            flagY = true;
        }

        if (animator.GetBool("is_setting"))    //戦闘態勢の時
        {
            Spine.RotateAround(spineTemp.position, this.transform.right, -rotateY);
            weaponBone.RotateAround(spineTemp.position, this.transform.right, -rotateY);
        }
        else
        {
            Neck.transform.Rotate(new Vector3(0, 0, -rotateY));
        }

        //Oculus Touchの右中指グリップを押し込んだ場合
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            //上半身のY軸角度をリセット
            rotateY = 0f;
        }
    }

    //通常態勢か戦闘態勢か
    void SwitchingCombatPosture()
    {
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))   //戦闘態勢への切替    Oculus Touchの左人差し指トリガーを押し込んだ場合
        {
            combatPosture = true;

            //遷移変数の初期化
            animator.SetBool("is_setting", true);
            animator.SetFloat("speed", 0f);
            animator.SetBool("is_jumping", false);
        }
        else                                //通常態勢への切替  Oculus Touchの左人差し指トリガーを押し込んでいない場合
        {
            combatPosture = false;

            //遷移変数の初期化
            animator.SetBool("is_setting", false);
            animator.SetBool("walk_front", false);
            animator.SetBool("walk_back", false);
            animator.SetBool("walk_right", false);
            animator.SetBool("walk_left", false);
            animator.SetBool("is_moving", false);
            animator.SetBool("is_shooting", false);
        }
    }

    //通常態勢の挙動
    void Normal()
    {
        if (stickL.y > 0.1f && !combatPosture)          //↑入力の検知かつ通常態勢の時
        {
            //前方向に走るアニメーションへ遷移
            animator.SetFloat("speed", 2f);
        }
        else if (stickL.y < -0.1f && !combatPosture)     //↓入力の検知かつ通常態勢の時
        {
            //前方向に走るアニメーションへ遷移
            animator.SetFloat("speed", -2f);
        }
        else                                            //上下入力が検知されなくて通常態勢の時
        {
            //後退させずに通常アニメーションへ遷移
            animator.SetFloat("speed", 0f);
        }

        //移動量の更新（通常アニメーション）
        gameObject.transform.Rotate(new Vector3(0, rotateSpeed * stickL.x, 0));
        moveDirection = runSpeed * stickL.y * gameObject.transform.forward;

        //Oculus Touchの左中指グリップを押し込んだ場合
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            //ジャンプアニメーションへ遷移
            animator.SetBool("is_jumping", true);
            moveDirection.y = jumpSpeed;
        }
    }

    //戦闘態勢の挙動
    void Combat()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))     //Oculus Touchの右人差し指トリガーを押し込んだ場合
        {
            //射撃遷移変数の初期化
            animator.SetBool("is_shooting", true);
        }
        else
        {
            //射撃遷移変数の初期化
            animator.SetBool("is_shooting", false);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B))                 //Oculus TouchのBボタンを押した場合
        {
            //リロード遷移変数の初期化
            animator.SetBool("is_reroading", true);
        }

        if (stickL.x > 0.1f || stickL.x < -0.1f
            || stickL.y > 0.1f || stickL.y < -0.1f)
        {
            //移動遷移変数の初期化
            animator.SetBool("is_moving", true);

            if (stickL.y > 0.1f && combatPosture)     //↑入力の検知かつ戦闘態勢の時
            {
                //移動遷移変数の初期化
                animator.SetBool("walk_back", false);
                animator.SetBool("walk_right", false);
                animator.SetBool("walk_left", false);

                //前進遷移変数の初期化
                animator.SetBool("walk_front", true);
            }
            else                        //↑入力が未検知の時
            {
                //前進遷移変数の初期化
                animator.SetBool("walk_front", false);
            }

            if (stickL.y < -0.1f && combatPosture)    //↓入力の検知かつ戦闘態勢の時
            {
                //移動遷移変数の初期化
                animator.SetBool("walk_front", false);
                animator.SetBool("walk_right", false);
                animator.SetBool("walk_left", false);

                //後退遷移変数の初期化
                animator.SetBool("walk_back", true);
            }
            else                        //↓入力が未検知の時
            {
                //後退遷移変数の初期化
                animator.SetBool("walk_back", false);
            }

            //左右移動アニメーション遷移の定義
            if (stickL.y < 0.2f && stickL.y > -0.2f)
            {
                if (stickL.x > 0.1f && combatPosture)     //→入力の検知かつ戦闘態勢の時
                {
                    //移動遷移変数の初期化
                    animator.SetBool("walk_front", false);
                    animator.SetBool("walk_back", false);
                    animator.SetBool("walk_left", false);

                    //右移動遷移変数の初期化
                    animator.SetBool("walk_right", true);
                }
                else                        //→入力が未検知の時
                {
                    //右移動遷移変数の初期化
                    animator.SetBool("walk_right", false);
                }

                if (stickL.x < -0.1f && combatPosture)    //←入力の検知かつ戦闘態勢の時
                {
                    //移動遷移変数の初期化
                    animator.SetBool("walk_front", false);
                    animator.SetBool("walk_back", false);
                    animator.SetBool("walk_right", false);

                    //左移動遷移変数の初期化
                    animator.SetBool("walk_left", true);
                }
                else                        //←入力が未検知の時
                {
                    //左移動遷移変数の初期化
                    animator.SetBool("walk_left", false);
                }
            }
        }
        else
        {
            //移動遷移変数の初期化
            animator.SetBool("is_moving", false);
            animator.SetBool("walk_front", false);
            animator.SetBool("walk_back", false);
            animator.SetBool("walk_right", false);
            animator.SetBool("walk_left", false);
        }

        //Oculus Touchの左中指グリップを押し込んだ場合
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            //ステップする
            isSteping = true;
        }

        if (isSteping == true)
        {
            if (stepTime > dTime)
            {
                runSpeed = stepSpeed;
                dTime += Time.deltaTime;
            }
            else
            {
                runSpeed = speed;
                dTime = 0.0f;
                isSteping = false;
            }
        }

        //移動量の更新（戦闘アニメーション）
        gameObject.transform.Rotate(new Vector3(0, rotateSpeed * 0.5f * stickR.x, 0));
        moveDirection = new Vector3(stickL.x, 0, stickL.y);
        moveDirection = runSpeed * transform.TransformDirection(moveDirection) * 0.5f;
    }

    //ダメージ計算
    public void TakeDamage(int attackedPower)
    {
        myStatus.SetDamage(attackedPower);
    }
}
