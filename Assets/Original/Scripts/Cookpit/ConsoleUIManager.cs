using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ConsoleUIManager : MonoBehaviour
{
    public GameObject[] button;

    [SerializeField]
    private ActivityLogUIManager ActivityLogPanel;
    [SerializeField]
    private EquipmentItemUIManager EquipmentItemPanel;
    [SerializeField]
    private ChoiceUIManager choicePanel;
    [SerializeField]
    private ResultUIManager resultPanel;
    
    [NonSerialized]
    public bool flag = false;           //このスクリプトを動かすフラグ
    [NonSerialized]
    public int number = 0;

    private ActiveConsole activeConsole;
    private CanvasGroup activityLogCanvas;
    private CanvasGroup equipmentItemCanvas;
    private CanvasGroup choiceCanvas;
    private CanvasGroup resultCanvas;
    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        activeConsole = GetComponent<ActiveConsole>();
        activityLogCanvas = ActivityLogPanel.gameObject.GetComponent<CanvasGroup>();
        activityLogCanvas.alpha = 0f;
        equipmentItemCanvas = EquipmentItemPanel.gameObject.GetComponent<CanvasGroup>();
        equipmentItemCanvas.alpha = 0f;
        choiceCanvas = choicePanel.gameObject.GetComponent<CanvasGroup>();
        choiceCanvas.alpha = 0f;
        resultCanvas = resultPanel.gameObject.GetComponent<CanvasGroup>();
        resultCanvas.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ConsoleUIManagerのflag = " + flag);
        if (flag)
        {
            transitionTime += Time.deltaTime;

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight) && number < button.Length - 1)
            {
                number++;
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft) && number > 0)
            {
                number--;
            }

            EventSystem.current.SetSelectedGameObject(button[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    activeConsole.flag = false;
                    activeConsole.openWindow = ActiveConsole.OpenWindow.Console;
                    transitionTime = 0f;

                    activityLogCanvas.alpha = 0f;
                    equipmentItemCanvas.alpha = 0f;
                    choiceCanvas.alpha = 0f;
                    resultCanvas.alpha = 0f;
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    if (number == 0)
                    {
                        if (button[number].GetComponent<ActivityLogUIManager>().logs.Count == 0)
                        {
                            return;
                        }
                        button[number].GetComponent<ActivityLogUIManager>().flag = true;
                    }
                    else if (number == 1)
                    {
                        button[number].GetComponent<EquipmentItemUIManager>().flag = true;
                    }

                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
    }
}
