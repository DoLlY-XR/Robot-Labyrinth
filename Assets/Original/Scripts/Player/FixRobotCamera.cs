using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRobotCamera : MonoBehaviour
{
    //ロボットのカメラ(仮)
    public Transform robotCameraTemp;
    //ロボットのカメラ
    public Transform robotCamera;

    private MyController myController;
    private float rotateY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //アバターのAvatorControllerコンポーネントを取得
        myController = GetComponent<MyController>();
    }

    protected virtual void LateUpdate()
    {
        rotateY = myController.stickR.y;

        if (!myController.flagY)
        {
            rotateY = 0f;
        }

        robotCameraTemp.RotateAround(myController.neckTemp.position, this.transform.right, -rotateY);
        robotCamera.transform.Rotate(new Vector3(-rotateY, 0, 0));
    }
}
