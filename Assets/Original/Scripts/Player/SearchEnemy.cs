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
        Debug.Log("OnTriggerStay");
        Debug.Log("col.tag = " + col.tag);
        Debug.Log("col.name = " + col.name);
        if (col.tag == "RadarTarget")
        {
            Debug.Log("不透明");
            var sprite = col.GetComponent<SpriteRenderer>();
            sprite.enabled = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("OnTriggerExit");
        Debug.Log("col.tag = " + col.tag);
        Debug.Log("col.name = " + col.name);
        if (col.tag == "RadarTarget")
        {
            Debug.Log("透明");
            var sprite = col.GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }
    }
}
