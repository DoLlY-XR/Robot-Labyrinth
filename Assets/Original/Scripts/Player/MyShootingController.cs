using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyShootingController : MonoBehaviour
{
    //ビームのパーティクルプレハブ
    [SerializeField]
    private ParticleSystem particlePrefab;
    //特殊ビームのパーティクルプレハブ
    [SerializeField]
    private ParticleSystem specialParticlePrefab;
    //銃口
    [SerializeField]
    private Transform muzzle;
    //エネルギータンクの情報
    [SerializeField]
    private ItemInformation energyTank;
    //高密度エネルギータンクの情報
    [SerializeField]
    private ItemInformation highEnergyTank;

    private MyController myController;
    private MyStatus myStatus;                      //プレイヤーのステータス管理スクリプト
    private Vector3 muzzleInitialPos;
    private float muzzleInitialAngleX;
    private float animationTime = 0f;
    private float specialWaitTime = 0f;
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
        if (!myController.cookpit.flag)
        {
            if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_SingleShot_AR") ||
            myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Reload") ||
            myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Autoshot_AR"))
            {
                dTime++;

                if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Autoshot_AR"))
                {
                    specialWaitTime += Time.deltaTime;

                    if (specialWaitTime > 1.5f)
                    {
                        myController.animator.SetBool("spacial_shooting", false);
                    }
                }
            }
            else
            {
                dTime = 0;
                specialWaitTime = 0f;
            }

            if (myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime - animationTime > 1f)
            {
                if (!myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Autoshot_AR"))
                {
                    dTime = 0;
                    animationTime = myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
                }
            }

            if (dTime == 1)            //銃撃時
            {
                animationTime = myController.animator.GetCurrentAnimatorStateInfo(1).normalizedTime;

                if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_SingleShot_AR") &&
                    myStatus.EnergyAmount > 0)
                {
                    myStatus.EnergyAmount--;

                    var particleInstance = Instantiate<ParticleSystem>
                            (particlePrefab, muzzle.position, muzzle.rotation);
                    particleInstance.transform.parent = muzzle;

                    BeamAttack beamAttack = particleInstance.GetComponent<BeamAttack>();
                    beamAttack.SetAttackPower(myStatus.AttackPower);

                    //パーティクルを開始
                    particleInstance.Play(true);
                }
                else if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
                {
                    myStatus.EnergyAmount = myStatus.MaxEnergyAmount;
                    energyTank.ItemQuantity--;
                }
                else if (myController.animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Autoshot_AR"))
                {
                    highEnergyTank.ItemQuantity--;

                    var particleInstance = Instantiate<ParticleSystem>
                            (specialParticlePrefab, muzzle.position, muzzle.rotation);
                    particleInstance.transform.parent = muzzle;

                    SpecialBeamAttack specialBeamAttack = particleInstance.GetComponent<SpecialBeamAttack>();
                    specialBeamAttack.SetAttackPower(myStatus.AttackPower);

                    //パーティクルを開始
                    particleInstance.Play(true);
                }
            }
        }
    }

    protected virtual void LateUpdate()
    {
        if (!myController.cookpit.flag)
        {
            rotateY = myController.stickR.y;

            if (!myController.flagY)
            {
                rotateY = 0f;
            }

            muzzle.RotateAround(myController.spineTemp.position, this.transform.right, -rotateY);

            //Oculus TouchのAボタンを押した場合
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                //銃口の座標・角度をリセット
                muzzle.localPosition = muzzleInitialPos;
                muzzle.eulerAngles = new Vector3(muzzleInitialAngleX, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            }
        }
    }
}
