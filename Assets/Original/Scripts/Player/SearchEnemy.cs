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
            var sprite = col.GetComponent<SpriteRenderer>();
            sprite.enabled = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "RadarTarget")
        {
            var sprite = col.GetComponent<SpriteRenderer>();
            sprite.enabled = false;
        }
    }
}
