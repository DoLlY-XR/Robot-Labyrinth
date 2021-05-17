using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChoiceUIManager : MonoBehaviour
{
    public Text choiceText;
    public GameObject[] button;
    public GameObject[] transition;
    public ItemListUIManager itemListUIManager;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ
    [NonSerialized]
    public int quantity = 0;
    [NonSerialized]
    public int number = 0;

    private CanvasGroup choiceCanvas;
    private DescriptionUIManager description;
    private ItemInformation itemInfo;
    private ItemInformation upgradeItemInfo;
    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        choiceCanvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            transitionTime += Time.deltaTime;
            choiceCanvas.alpha = 1f;

            button[0].transform.Find("Quantity").GetComponent<Text>().text = "× " + quantity;
            itemInfo = itemListUIManager.items[itemListUIManager.number].GetComponent<ItemInformation>();
            if (itemInfo.Item.upgradeItem != null)
            {
                upgradeItemInfo = itemInfo.Item.upgradeItem.GetComponent<ItemInformation>();
            }
            description = transition[0].GetComponent<DescriptionUIManager>();

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight) && number < button.Length - 1 && quantity > 0)
            {
                number++;
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0, 1);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft) && number > 0)
            {
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 1);
                number--;
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number == 0)
            {
                if (description.number == 0)
                {
                    if (itemInfo.Item.quantity / itemInfo.Item.upgradeQuantity > quantity &&
                        upgradeItemInfo.Item.maxQuantity - upgradeItemInfo.Item.quantity > quantity)
                    {
                        quantity++;
                    }
                }
                else if (description.number == 1)
                {
                    if (itemInfo.Item.quantity > quantity)
                    {
                        quantity++;
                    }
                }
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number == 0 && quantity > 0)
            {
                quantity--;
            }

            EventSystem.current.SetSelectedGameObject(button[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    transition[0].GetComponent<DescriptionUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                    quantity = 0;
                    number = 0;
                    button[1].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    button[1].transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 1);
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    if (number == 1 && quantity > 0)
                    {
                        transition[1].GetComponent<ResultUIManager>().flag = true;
                        this.flag = false;
                        description.choiceFlag = false;
                        transitionTime = 0f;
                        number = 0;
                        button[1].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                        button[1].transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 1);
                    }
                }
            }
        }
        else
        {
            choiceCanvas.alpha = 0f;
        }
    }
}
