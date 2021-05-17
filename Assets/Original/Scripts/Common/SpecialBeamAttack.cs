using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBeamAttack : MonoBehaviour
{
    //攻撃がヒットした相手のタグリスト
    public enum HitTag
    {
        Player,
        Enemy
    };

    //ダメージ処理するオブジェクトのタグ
    [SerializeField]
    private HitTag hitTag;

    private int attackPower;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log("obj.name = " + obj.name);
        Debug.Log("obj.tag = " + obj.tag);
        if (obj.tag == hitTag.ToString())
        {
            count++;
            if (count % 10 == 0)
            {
                if (hitTag == HitTag.Player)
                {
                    var Player = obj.transform.root.GetComponent<MyController>();
                    Player.TakeDamage((float)attackPower);
                }
                if (hitTag == HitTag.Enemy)
                {
                    var enemy = obj.GetComponent<EnemyController>();
                    if (enemy.GetState() != EnemyController.EnemyState.Dead)
                    {
                        enemy.TakeDamage((float)attackPower, this.transform.root);
                    }
                }
            }
        }
    }

    public void SetAttackPower(int attackPower)
    {
        this.attackPower = attackPower;
    }
}
