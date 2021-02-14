using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingControl : MonoBehaviour
{
    //ビームのパーティクルプレハブ
    public ParticleSystem particlePrefab;
    //銃口
    public Transform muzzle;
    //ビームのコライダープレハブ
    public GameObject colliderPrefab;
    //ビームコライダーを飛ばす力
    public float colliderPower = 15000f;
    //ビームコライダーの寿命
    public float colliderLifeTime = 0.5f;

    private AvatorController script;
    private float animationTime = 0f;
    private int dTime1 = 0;
    private int dTime2 = 0;
    private float rotateY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //アバターのAvatorControllerコンポーネントを取得
        script = GetComponent<AvatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (script.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_SingleShot_AR"))
        {
            dTime1++;
        }
        else
        {
            dTime1 = 0;
        }

        if (script.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - animationTime > 1f)
        {
            dTime1 = 0;
            animationTime = script.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        }

        if (dTime1 == 1)            //戦闘態勢　Oculus Touchの右人差し指トリガーを押し込んだ場合
        {
            animationTime = script.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;

            var particleInstance = Instantiate<ParticleSystem>
                    (particlePrefab, muzzle.position, muzzle.rotation);

            //パーティクルを開始
            particleInstance.Play(true);

            var colliderInstance = Instantiate<GameObject>(colliderPrefab, muzzle.position, muzzle.rotation);
            colliderInstance.GetComponent<Rigidbody>().AddForce(colliderInstance.transform.forward * colliderPower);
            Destroy(colliderInstance, colliderLifeTime);
        }
    }

    protected virtual void LateUpdate()
    {
        if (script.animator.GetBool("is_setting"))    //戦闘態勢の時
        {
            dTime2 = 0;
            rotateY = script.stickR.y;

            if (!script.flag)
            {
                rotateY = 0f;
            }

            muzzle.RotateAround(script.spineTemp.position, this.transform.right, -rotateY);
        }
        else
        {
            dTime2++;

            if (dTime2 == 1)
            {
                muzzle.RotateAround(script.spineTemp.position, this.transform.right, script.tempRotateY);
            }
        }
    }
}
