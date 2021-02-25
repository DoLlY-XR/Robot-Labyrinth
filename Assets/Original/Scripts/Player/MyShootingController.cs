using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyShootingController : MonoBehaviour
{
    //ビームのパーティクルプレハブ
    public ParticleSystem particlePrefab;
    //銃口
    public Transform muzzle;

    private MyController myController;
    private MyStatus myStatus;                      //プレイヤーのステータス管理スクリプト
    private Vector3 muzzleInitialPos;
    private float muzzleInitialAngleX;
    private float animationTime = 0f;
    private int dTime = 0;
    private float rotateY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのAvatorControllerコンポーネントを取得
        myController = GetComponent<MyController>();
        //プレイヤーのMyStatusコンポーネントを取得
        myStatus = GetComponent<MyStatus>();
        //銃口の初期位置・角度を取得
        muzzleInitialPos = muzzle.localPosition;
        muzzleInitialAngleX = muzzle.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_SingleShot_AR"))
        {
            dTime++;
        }
        else
        {
            dTime = 0;
        }

        if (myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - animationTime > 1f)
        {
            dTime = 0;
            animationTime = myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        }

        if (dTime == 1)            //戦闘態勢　Oculus Touchの右人差し指トリガーを押し込んだ場合
        {
            animationTime = myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
            
            var particleInstance = Instantiate<ParticleSystem>
                    (particlePrefab, muzzle.position, muzzle.rotation);
            
            BeamAttack beamAttack = particleInstance.GetComponent<BeamAttack>();
            beamAttack.SetAttackPower(myStatus.GetAttackPower());

            //パーティクルを開始
            particleInstance.Play(true);
        }
    }

    protected virtual void LateUpdate()
    {
        rotateY = myController.stickR.y;

        if (!myController.flagY)
        {
            rotateY = 0f;
        }

        muzzle.RotateAround(myController.spineTemp.position, this.transform.right, -rotateY);

        //Oculus Touchの右中指グリップを押し込んだ場合
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            //銃口の座標・角度をリセット
            muzzle.localPosition = muzzleInitialPos;
            muzzle.eulerAngles = new Vector3(muzzleInitialAngleX, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        }
    }
}
