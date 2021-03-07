using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipmentItemUIManager : MonoBehaviour
{
    public GameObject[] button;

    [SerializeField]
    private ChoiceUIManager choicePanel;
    [SerializeField]
    private ResultUIManager resultPanel;
    [SerializeField]
    private ActiveDisplay cookpit;

    [NonSerialized]
    public bool flag = false;           //このスクリプトを動かすフラグ
    [NonSerialized]
    public int number = 0;

    private CanvasGroup equipmentItemCanvas;
    private CanvasGroup choiceCanvas;
    private CanvasGroup resultCanvas;
    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        equipmentItemCanvas = GetComponent<CanvasGroup>();
        equipmentItemCanvas.alpha = 0f;
        choiceCanvas = choicePanel.gameObject.GetComponent<CanvasGroup>();
        choiceCanvas.alpha = 0f;
        resultCanvas = resultPanel.gameObject.GetComponent<CanvasGroup>();
        resultCanvas.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("EquipmentItemUIManagerのflag = " + flag);
        if (flag)
        {
            transitionTime += Time.deltaTime;

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number < button.Length - 1)
            {
                number++;
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number > 0)
            {
                number--;
            }

            EventSystem.current.SetSelectedGameObject(button[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    this.flag = false;
                    cookpit.flag = false;
                    transitionTime = 0f;

                    equipmentItemCanvas.alpha = 0f;
                    choiceCanvas.alpha = 0f;
                    resultCanvas.alpha = 0f;
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    if (number == 0)
                    {
                        button[number].GetComponent<EquipmentUIManager>().flag = true;
                    }
                    else if (number == 1)
                    {
                        button[number].GetComponent<ItemListUIManager>().flag = true;
                    }

                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
    }
}
