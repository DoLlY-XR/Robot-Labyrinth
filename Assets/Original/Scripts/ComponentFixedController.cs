using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFixedController : MonoBehaviour
{
    public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Pos = this.transform.position;
        Pos.y = gameObject.transform.position.y;
        this.transform.position = Pos;
    }
}
