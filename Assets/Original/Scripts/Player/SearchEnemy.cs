using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "RadarTarget")
        {
            Debug.Log("レーダー外 col.name = " + col.name);
            if (col.transform.parent.GetComponent<EnemyController>().enemyState == EnemyController.EnemyState.Attack ||
                col.transform.parent.GetComponent<EnemyController>().enemyState == EnemyController.EnemyState.Chase ||
                col.transform.parent.GetComponent<EnemyController>().enemyState == EnemyController.EnemyState.Freeze)
            {
                col.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
            }
            else if (col.transform.parent.GetComponent<EnemyController>().enemyState == EnemyController.EnemyState.Idle ||
                col.transform.parent.GetComponent<EnemyController>().enemyState == EnemyController.EnemyState.Walk)
            {
                col.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 1f);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "RadarTarget")
        {
            Debug.Log("レーダー外 col.name = " + col.name);
            var spriteColor = col.GetComponent<SpriteRenderer>().color;
            var rColor = spriteColor.r;
            var gColor = spriteColor.g;
            var bColor = spriteColor.b;
            col.GetComponent<SpriteRenderer>().color = new Color(rColor, gColor, bColor, 0f);
        }
    }
}
