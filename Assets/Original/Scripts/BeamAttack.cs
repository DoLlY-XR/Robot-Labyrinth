using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //　コライダーのIsTriggerのチェックを入れ物理的な衝突をしない（突き抜ける）場合
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
}
