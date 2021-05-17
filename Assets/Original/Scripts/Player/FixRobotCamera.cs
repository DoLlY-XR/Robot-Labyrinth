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
    private Vector3 cameraInitialPos;
    private float cameraInitialAngleX;
    private float rotateY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //アバターのAvatorControllerコンポーネントを取得
        myController = GetComponent<MyController>();
        //カメラの初期位置・角度を取得
        cameraInitialPos = robotCamera.localPosition;
        cameraInitialAngleX = robotCamera.eulerAngles.x;
    }

    protected virtual void LateUpdate()
    {
        if (myController.gameManager.gameStatus == GameManager.GameStatus.Over || myController.gameManager.gameStatus == GameManager.GameStatus.Clear)
        {
            return;
        }

        if (!myController.activeConsole.flag)
        {
            rotateY = myController.stickR.y;

            if (!myController.flagY)
            {
                rotateY = 0f;
            }

            robotCameraTemp.RotateAround(myController.neckTemp.position, this.transform.right, -rotateY);
            robotCamera.transform.Rotate(new Vector3(-rotateY, 0, 0));

            //Oculus TouchのAボタンを押した場合
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                //カメラの座標・角度をリセット
                robotCameraTemp.localPosition = cameraInitialPos;
                robotCamera.eulerAngles = new Vector3(cameraInitialAngleX, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            }
        }
    }
}
