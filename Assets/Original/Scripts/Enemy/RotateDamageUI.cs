using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDamageUI : MonoBehaviour
{
    //ロボットのカメラ
    public GameObject robotCamera;

    // Start is called before the first frame update
    void Start()
    {
        robotCamera = GameObject.Find("RobotCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = robotCamera.transform.rotation;
    }
}
