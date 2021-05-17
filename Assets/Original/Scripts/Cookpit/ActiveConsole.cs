using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveConsole : MonoBehaviour
{
    public enum OpenWindow
    {
        Console,
        ActivityLogPanel,
        EquipmentItemPanel,
        EquipmentField,
        PartsListField,
        ItemListField,
        DescriptionField,
        ChoicePanel,
        ResultPanel
    }

    [SerializeField]
    private ActivityLogUIManager activityLogPanel;
    [SerializeField]
    private EquipmentItemUIManager equipmentItemPanel;
    [SerializeField]
    private EquipmentUIManager equipmentField;
    [SerializeField]
    private PartsListUIManager partsListField;
    [SerializeField]
    private ItemListUIManager itemListField;
    [SerializeField]
    private DescriptionUIManager descriptionField;
    [SerializeField]
    private ChoiceUIManager choicePanel;
    [SerializeField]
    private ResultUIManager resultPanel;
    [SerializeField]
    private GameObject player;

    [NonSerialized]
    public bool flag = false;
    [NonSerialized]
    public OpenWindow openWindow;

    private ConsoleUIManager consoleUIManager;
    private CanvasGroup activityLogCanvas;
    private CanvasGroup equipmentItemCanvas;
    private CanvasGroup choiceCanvas;
    private CanvasGroup resultCanvas;
    private MyController myController;

    // Start is called before the first frame update
    void Start()
    {
        openWindow = OpenWindow.Console;
        consoleUIManager = GetComponent<ConsoleUIManager>();
        activityLogCanvas = activityLogPanel.gameObject.GetComponent<CanvasGroup>();
        activityLogCanvas.alpha = 0f;
        equipmentItemCanvas = equipmentItemPanel.gameObject.GetComponent<CanvasGroup>();
        equipmentItemCanvas.alpha = 0f;
        choiceCanvas = choicePanel.gameObject.GetComponent<CanvasGroup>();
        choiceCanvas.alpha = 0f;
        resultCanvas = resultPanel.gameObject.GetComponent<CanvasGroup>();
        resultCanvas.alpha = 0f;
        myController = player.GetComponent<MyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myController.gameManager.gameStatus != GameManager.GameStatus.Progress)
        {
            activityLogCanvas.alpha = 0f;
            equipmentItemCanvas.alpha = 0f;
            choiceCanvas.alpha = 0f;
            resultCanvas.alpha = 0f;

            return;
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            if (!flag)
            {
                activityLogCanvas.alpha = 1f;
                equipmentItemCanvas.alpha = 1f;

                if (openWindow == OpenWindow.Console)
                {
                    consoleUIManager.flag = true;
                }
                else if (openWindow == OpenWindow.ActivityLogPanel)
                {
                    activityLogPanel.flag = true;
                }
                else if (openWindow == OpenWindow.EquipmentItemPanel)
                {
                    equipmentItemPanel.flag = true;
                }
                else if (openWindow == OpenWindow.EquipmentField)
                {
                    equipmentField.flag = true;
                }
                else if (openWindow == OpenWindow.PartsListField)
                {
                    partsListField.flag = true;
                }
                else if (openWindow == OpenWindow.ItemListField)
                {
                    itemListField.flag = true;
                }
                else if (openWindow == OpenWindow.DescriptionField)
                {
                    descriptionField.flag = true;
                }
                else if (openWindow == OpenWindow.ChoicePanel)
                {
                    choiceCanvas.alpha = 1f;
                    choicePanel.flag = true;
                }
                else if (openWindow == OpenWindow.ResultPanel)
                {
                    choiceCanvas.alpha = 1f;
                    resultCanvas.alpha = 1f;
                    resultPanel.flag = true;
                }

                flag = true;
            }
            else if (flag)
            {
                activityLogCanvas.alpha = 0f;
                equipmentItemCanvas.alpha = 0f;
                choiceCanvas.alpha = 0f;
                resultCanvas.alpha = 0f;

                if (consoleUIManager.flag)
                {
                    openWindow = OpenWindow.Console;
                    consoleUIManager.flag = false;
                }
                else if (activityLogPanel.flag)
                {
                    openWindow = OpenWindow.ActivityLogPanel;
                    activityLogPanel.flag = false;
                }
                else if (equipmentItemPanel.flag)
                {
                    openWindow = OpenWindow.EquipmentItemPanel;
                    equipmentItemPanel.flag = false;
                }
                else if (equipmentField.flag)
                {
                    openWindow = OpenWindow.EquipmentField;
                    equipmentField.flag = false;
                }
                else if (partsListField.flag)
                {
                    openWindow = OpenWindow.PartsListField;
                    partsListField.flag = false;
                }
                else if (itemListField.flag)
                {
                    openWindow = OpenWindow.ItemListField;
                    itemListField.flag = false;
                }
                else if (descriptionField.flag)
                {
                    openWindow = OpenWindow.DescriptionField;
                    descriptionField.flag = false;
                }
                else if (choicePanel.flag)
                {
                    openWindow = OpenWindow.ChoicePanel;
                    choicePanel.flag = false;
                }
                else if (resultPanel.flag)
                {
                    openWindow = OpenWindow.ResultPanel;
                    resultPanel.flag = false;
                }

                flag = false;
            }
        }
    }
}
