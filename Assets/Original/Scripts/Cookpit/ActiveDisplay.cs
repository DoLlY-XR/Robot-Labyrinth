using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveDisplay : MonoBehaviour
{
    [SerializeField]
    private EquipmentItemUIManager equipmentItemPanel;
    [SerializeField]
    private ChoiceUIManager choicePanel;
    [SerializeField]
    private ResultUIManager resultPanel;
    [SerializeField]
    private GameObject player;

    [NonSerialized]
    public bool flag = false;

    private CanvasGroup equipmentItemCanvas;
    private CanvasGroup choiceCanvas;
    private CanvasGroup resultCanvas;
    private MyStatus myStatus;

    // Start is called before the first frame update
    void Start()
    {
        myStatus = player.GetComponent<MyStatus>();
        equipmentItemCanvas = equipmentItemPanel.gameObject.GetComponent<CanvasGroup>();
        equipmentItemCanvas.alpha = 0f;
        choiceCanvas = choicePanel.gameObject.GetComponent<CanvasGroup>();
        choiceCanvas.alpha = 0f;
        resultCanvas = resultPanel.gameObject.GetComponent<CanvasGroup>();
        resultCanvas.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myStatus.Hp == 0)
        {
            equipmentItemCanvas.alpha = 0f;
            choiceCanvas.alpha = 0f;
            resultCanvas.alpha = 0f;

            return;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            if (!equipmentItemPanel.flag)
            {
                equipmentItemCanvas.alpha = 1f;
                equipmentItemPanel.flag = true;

                if (choicePanel.flag == true)
                {
                    choiceCanvas.alpha = 1f;
                }
                else if (resultPanel.flag == true)
                {
                    choiceCanvas.alpha = 1f;
                    resultCanvas.alpha = 1f;
                }

                flag = true;
            }
            else if (equipmentItemPanel.flag)
            {
                equipmentItemCanvas.alpha = 0f;
                choiceCanvas.alpha = 0f;
                resultCanvas.alpha = 0f;

                equipmentItemPanel.flag = false;

                flag = false;
            }
        }
    }
}
