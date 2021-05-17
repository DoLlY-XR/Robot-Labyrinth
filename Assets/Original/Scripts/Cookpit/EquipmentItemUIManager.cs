using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipmentItemUIManager : MonoBehaviour
{
    public GameObject[] button;
    public GameObject transition;

    [NonSerialized]
    public bool flag = false;           //このスクリプトを動かすフラグ
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
                    transition.GetComponent<ConsoleUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
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
