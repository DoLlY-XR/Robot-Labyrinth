using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatorController : MonoBehaviour
{
    public Transform upperBodyBone;
    public Transform weaponBone;
    public float speed = 6.0F;          //歩行速度
    public float jumpSpeed = 8.0F;      //ジャンプ力
    public float gravity = 20.0F;       //重力の大きさ
    public float rotateSpeed = 3.0F;    //回転速度

    private CharacterController controller;         //アバターのコントローラー
    private Animator animator;                      //アバターのアニメーター
    private Vector3 moveDirection = Vector3.zero;   //アバターの移動量
    private float h, v;                             //矢印キー
    private bool combatPosture = false;             //戦闘態勢の切替フラグ
    private float x = 0.0f;
    private float y = 0.0f;

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
        //h = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)
        //v = Input.GetAxis("Vertical");      //上下矢印キーの値(-1.0~1.0)

        //アバターの接地判定
        if (controller.isGrounded)
        {
            SwitchingCombatPosture();

            if (!animator.GetBool("is_setting"))    //戦闘アニメーションから通常アニメーションへの遷移時
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
        if (Input.GetKey(KeyCode.Alpha3))
        {
            x += Input.GetAxis("Horizontal");
            y += Input.GetAxis("Vertical");

            if (x > 30.0f)
            {
                x = 30.0f;
            }
            else if (x < -30.0f)
            {
                x = -30.0f;
            }
            if (y > 15.0f)
            {
                y = 15.0f;
            }
            else if (y < -15.0f)
            {
                y = -15.0f;
            }

            upperBodyBone.RotateAround(upperBodyBone.position, upperBodyBone.right, x * 3.0f);
            upperBodyBone.RotateAround(upperBodyBone.position, upperBodyBone.forward, y * 3.0f);

            weaponBone.RotateAround(upperBodyBone.position, upperBodyBone.right, x * 3.0f);
            weaponBone.RotateAround(upperBodyBone.position, upperBodyBone.forward, y * 3.0f);
        }
        else
        {
            x = 0.0f;
            y = 0.0f;
        }
    }

    //通常態勢か戦闘態勢か
    void SwitchingCombatPosture()
    {
        if (Input.GetKey(KeyCode.Alpha1))   //戦闘態勢への切替
        {
            combatPosture = true;

            //遷移変数の初期化
            animator.SetBool("is_setting", true);
            animator.SetBool("is_running", false);
            animator.SetBool("is_jumping", false);
        }
        else                                //通常態勢への切替
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
        h = 0.0f;
        v = Input.GetAxis("Vertical");      //上下矢印キーの値(-1.0~1.0)

        if (v > 0.1f && !combatPosture)    //↑入力の検知かつ通常態勢の時
        {
            h = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)
            
            //走るアニメーションへ遷移
            animator.SetBool("is_running", true);
        }
        else                        //↑入力が検知されない時
        {
            //後退させずに通常アニメーションへ遷移
            v = 0.0f;
            animator.SetBool("is_running", false);
        }

        //移動量の更新（通常アニメーション）
        gameObject.transform.Rotate(new Vector3(0, rotateSpeed * h, 0));
        moveDirection = speed * v * gameObject.transform.forward;
        moveDirection *= speed;

        //ジャンプボタンが押された時
        if (Input.GetButton("Jump"))
        {
            //ジャンプアニメーションへ遷移
            animator.SetBool("is_jumping", true);
            moveDirection.y = jumpSpeed;
        }
    }

    //戦闘態勢の挙動
    void Combat()
    {
        h = 0.0f;
        v = 0.0f;

        if (Input.GetKey(KeyCode.Space))
        {
            //射撃遷移変数の初期化
            animator.SetBool("is_shooting", true);
        }
        else
        {
            //射撃遷移変数の初期化
            animator.SetBool("is_shooting", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //リロード遷移変数の初期化
            animator.SetBool("is_reroading", true);
        }

        if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f
            || Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f)
        {
            //移動遷移変数の初期化
            animator.SetBool("is_moving", true);

            if (Input.GetAxis("Vertical") > 0.1f && combatPosture)     //↑入力の検知かつ戦闘態勢の時
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

            if (Input.GetAxis("Vertical") < -0.1f && combatPosture)    //↓入力の検知かつ戦闘態勢の時
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
            if (Input.GetAxis("Vertical") < 0.2f
                && Input.GetAxis("Vertical") > -0.2f)
            {
                if (Input.GetAxis("Horizontal") > 0.1f && combatPosture)     //→入力の検知かつ戦闘態勢の時
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

                if (Input.GetAxis("Horizontal") < -0.1f && combatPosture)    //←入力の検知かつ戦闘態勢の時
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
        

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkFront_Shoot_AR")
            && Input.GetAxis("Vertical") > 0.0f)
        {
            h = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)
            v = Input.GetAxis("Vertical");      //上矢印キーの値(0.0~1.0)
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkBack_Shoot_AR")
            && Input.GetAxis("Vertical") < 0.0f)
        {
            h = Input.GetAxis("Horizontal");    //左右矢印キーの値(-1.0~1.0)
            v = Input.GetAxis("Vertical");      //下矢印キーの値(-1.0~0.0)
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRight_Shoot_AR")
            && Input.GetAxis("Horizontal") > 0.0f)
        {
            h = Input.GetAxis("Horizontal");    //右矢印キーの値(0.0~1.0)
            v = Input.GetAxis("Vertical");      //上下矢印キーの値(-1.0~1.0)
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLeft_Shoot_AR")
            && Input.GetAxis("Horizontal") < 0.0f)
        {
            h = Input.GetAxis("Horizontal");    //左矢印キーの値(-1.0~0.0)
            v = Input.GetAxis("Vertical");      //上下矢印キーの値(-1.0~1.0)
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            v = 0.0f;       //エイム移動中は移動させない

            //移動量の更新（通常アニメーション）
            gameObject.transform.Rotate(new Vector3(0, rotateSpeed * 0.5f * h, 0));
            moveDirection = speed * v * gameObject.transform.forward;
        }
        else
        {
            //移動量の更新（戦闘アニメーション）
            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
        }
        
        moveDirection *= speed;
    }
}
