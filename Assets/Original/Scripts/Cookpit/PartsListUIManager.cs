using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PartsListUIManager : MonoBehaviour
{
    public List<GameObject> parts;
    public Transform content;
    public ScrollRect scrollView;
    public GameObject transition;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ
    [NonSerialized]
    public int number = 0;

    private EquipmentUIManager equipmentUIManager;
    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in content)
        {
            parts.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("PartsListUIManagerのflag = " + flag);
        parts.Clear();

        foreach (Transform child in content)
        {
            if (child.gameObject.activeSelf)
            {
                parts.Add(child.gameObject);
            }
        }

        if (flag && this.gameObject.activeSelf)
        {
            transitionTime += Time.deltaTime;
            equipmentUIManager = transition.GetComponent<EquipmentUIManager>();

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number < parts.Count - 1)
            {
                number++;
                scrollView.verticalNormalizedPosition -= 1f / (parts.Count - 1f);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number > 0)
            {
                number--;
                scrollView.verticalNormalizedPosition += 1f / (parts.Count - 1f);
            }

            EventSystem.current.SetSelectedGameObject(parts[number]);
            var partsInformation = parts[number].GetComponent<PartsInformation>();

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    equipmentUIManager.flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) &&
                    partsInformation.Parts.partsType == equipmentUIManager.button[equipmentUIManager.number].GetComponent<PartsInformation>().Parts.partsType)
                {
                    var temp = equipmentUIManager.button[equipmentUIManager.number].GetComponent<PartsInformation>().Parts;
                    equipmentUIManager.button[equipmentUIManager.number].GetComponent<PartsInformation>().Parts = partsInformation.Parts;
                    partsInformation.Parts = temp;
                    equipmentUIManager.flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
    }

    public void AddParts(GameObject newParts)
    {
        GameObject addParts = Instantiate<GameObject>(newParts, content.transform);
        parts.Add(addParts);
    }
}
