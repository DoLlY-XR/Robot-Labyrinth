using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipmentUIManager : MonoBehaviour
{
    public GameObject[] button;
    public GameObject[] transition;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ
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
        Debug.Log("EquipmentUIManagerのflag = " + flag);
        if (flag && this.gameObject.activeSelf)
        {
            transitionTime += Time.deltaTime;
            var partsList = transition[1].GetComponent<PartsListUIManager>();

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number < button.Length - 1)
            {
                number++;
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number > 0)
            {
                number--;
            }

            foreach (Transform child in partsList.content)
            {
                if (child.GetComponent<PartsInformation>().Parts.partsType != button[number].GetComponent<PartsInformation>().Parts.partsType)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }

            EventSystem.current.SetSelectedGameObject(button[number]);

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    transition[0].GetComponent<EquipmentItemUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;

                    partsList.parts.Clear();

                    foreach (Transform child in partsList.content)
                    {
                        partsList.parts.Add(child.gameObject);
                        child.gameObject.SetActive(true);
                    }
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    if (partsList.parts.Count > 0)
                    {
                        partsList.flag = true;
                        this.flag = false;
                        transitionTime = 0f;
                    }
                }
            }
        }
    }
}
