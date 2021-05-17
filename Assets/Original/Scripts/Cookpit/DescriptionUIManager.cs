using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DescriptionUIManager : MonoBehaviour
{
    public Text descriptionText;
    public GameObject[] button;
    public GameObject[] transition;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ
    [NonSerialized]
    public bool choiceFlag = false;
    [NonSerialized]
    public int number = 0;

    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DescriptionUIManagerのflag = " + flag);
        if (flag)
        {
            transitionTime += Time.deltaTime;

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight) && number < button.Length - 1)
            {
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 1);
                number++;
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0, 1);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft) && number > 0)
            {
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(1, 1, 1, 1);
                number--;
                button[number].transform.Find("Background").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                button[number].transform.Find("Text").GetComponent<Text>().color = new Color(0, 0, 0, 1);
            }

            EventSystem.current.SetSelectedGameObject(button[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    transition[0].GetComponent<ItemListUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    var itemListUIManager = transition[0].GetComponent<ItemListUIManager>();
                    var itemInfo = itemListUIManager.items[itemListUIManager.number].GetComponent<ItemInformation>();

                    if (number == 0)
                    {
                        if (itemInfo.Item.upgradeQuantity == 0)
                        {
                            return;
                        }
                        transition[1].GetComponent<ChoiceUIManager>().flag = true;
                        choiceFlag = true;
                    }
                    else if (number == 1)
                    {
                        if (itemInfo.ItemQuantity == 0)
                        {
                            return;
                        }
                        transition[2].GetComponent<ResultUIManager>().flag = true;
                    }
                    
                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
    }
}
