using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ActivityLogUIManager : MonoBehaviour
{
    public List<GameObject> logs;
    public GameObject transition;
    public ScrollRect scrollView;
    public Transform content;

    [NonSerialized]
    public bool flag = false;           //このスクリプトを動かすフラグ
    [NonSerialized]
    public int number = 0;

    private ActiveConsole console;
    private CanvasGroup activityLogCanvas;
    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        console = GetComponent<ActiveConsole>();
        activityLogCanvas = GetComponent<CanvasGroup>();
        activityLogCanvas.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ActivityLogUIManagerのflag = " + flag);
        if (flag)
        {
            transitionTime += Time.deltaTime;

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number < logs.Count - 1)
            {
                number++;
                scrollView.verticalNormalizedPosition -= 1f / (logs.Count - 1f);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number > 0)
            {
                number--;
                scrollView.verticalNormalizedPosition += 1f / (logs.Count - 1f);
            }

            EventSystem.current.SetSelectedGameObject(logs[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    transition.GetComponent<ConsoleUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
    }

    public void AddLog(GameObject newLog)
    {
        GameObject addLog = Instantiate<GameObject>(newLog, content.transform);
        logs.Add(addLog);
    }
}
