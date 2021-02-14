using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRobotCamera : MonoBehaviour
{
    [SerializeField]
    private Transform avatar;       //ロボットのアバター

    void LateUpdate()
    {
        Vector3 angles = avatar.transform.eulerAngles;

        
        //視点の角度をavatarに修正
        transform.eulerAngles = angles;
    }
}
