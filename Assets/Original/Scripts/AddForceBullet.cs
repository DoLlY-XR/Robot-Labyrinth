using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceBullet : MonoBehaviour
{
    //　弾のゲームオブジェクト
    [SerializeField]
    private GameObject bulletPrefab;
    //　銃口
    [SerializeField]
    private Transform muzzle;
    //　弾を飛ばす力
    [SerializeField]
    private float bulletPower = 500.0f;

    //　アバターのアニメーター
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //アバターのAnimatorコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot_Autoshot_AR"))
        {
            Shot();
        }
    }

    //　敵を撃つ
    void Shot()
    {
        var bulletInstance = Instantiate<GameObject>(bulletPrefab,
            muzzle.position, muzzle.rotation);
        bulletInstance.GetComponent<Rigidbody>()
            .AddForce(bulletInstance.transform.forward * bulletPower);
        Destroy(bulletInstance, 1f);
    }
}
