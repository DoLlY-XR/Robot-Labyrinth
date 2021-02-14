using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatorController : MonoBehaviour
{
    public Transform spineTemp;
    public Transform Spine;
    public Transform weaponBone;
    public float speed = 6.0F;                      //歩行速度
    public float jumpSpeed = 8.0F;                  //ジャンプ力
    public float gravity = 20.0F;                   //重力の大きさ
    public float rotateSpeed = 3.0F;                //回転速度
    public float postureRotateLimit = 30f;          //銃撃姿勢で回転出来る限度

    [NonSerialized]
    public Animator animator;                       //ロボットのアニメーター
    [NonSerialized]
    public float tempRotateY = 0.0f;                //ShootingControlスクリプトで使うrotateY
    [NonSerialized]
    public Vector2 stickL;                          //左手のアナログスティック
    [NonSerialized]
    public Vector2 stickR;                          //右手のアナログスティック
    [NonSerialized]
    public bool flag = true;                        //ShootingControlスクリプトで使うフラグ

    private CharacterController controller;         //ロボットのコントローラー
    private Vector3 moveDirection = Vector3.zero;   //ロボットの移動量
    private bool combatPosture = false;             //戦闘態勢の切替フラグ
    public float rotateY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //アバターのCharacterControllerコンポーネントを取得
        controller = GetComponent<CharacterController>();

        //アバターのAnimatorコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        stickL = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);      // 左手のアナログスティックの向きを取得
        stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);      // 右手のアナログスティックの向きを取得

        //アバターの接地判定
        if (controller.isGrounded)
        {
            SwitchingCombatPosture();

            if (!animator.GetBool("is_setting"))        //戦闘アニメーションから通常アニメーションへの遷移時
            {
                Normal();
            }
            else if (animator.GetBool("is_setting"))    //通常アニメーションから戦闘アニメーションへの遷移時
            {
                Combat();
            }
        }

        //アバターの座標移動
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    protected virtual void LateUpdate()
    {
        if (animator.GetBool("is_setting"))    //戦闘態勢の時
        {
            rotateY += stickR.y;
            tempRotateY = rotateY;

            if (rotateY > postureRotateLimit)
            {
                rotateY = postureRotateLimit;
                flag = false;
            }
            else if (rotateY < -postureRotateLimit)
            {
                rotateY = -postureRotateLimit;
                flag = false;
            }
            else
            {
                flag = true;
            }

            Spine.RotateAround(spineTemp.position, this.transform.right, -rotateY);
            weaponBone.RotateAround(spineTemp.position, this.transform.right, -rotateY);
        }
        else
        {
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
            animator.SetBool("is_running", false);
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
        if (stickL.y > 0.1f && !combatPosture)    //↑入力の検知かつ通常態勢の時
        {
            //走るアニメーションへ遷移
            animator.SetBool("is_running", true);
        }
        else                        //↑入力が検知されない時
        {
            //後退させずに通常アニメーションへ遷移
            stickL.y = 0f;
            animator.SetBool("is_running", false);
        }

        //移動量の更新（通常アニメーション）
        gameObject.transform.Rotate(new Vector3(0, rotateSpeed * stickL.x, 0));
        moveDirection = speed * stickL.y * gameObject.transform.forward;

        //Oculus Touchの左中指グリップを押し込んだ場合
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
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

        //移動量の更新（戦闘アニメーション）
        gameObject.transform.Rotate(new Vector3(0, rotateSpeed * 0.5f * stickR.x, 0));
        moveDirection = new Vector3(stickL.x, 0, stickL.y);
        moveDirection = speed * transform.TransformDirection(moveDirection) * 0.5f;
    }
}
