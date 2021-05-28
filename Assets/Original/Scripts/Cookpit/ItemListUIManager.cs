using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemListUIManager : MonoBehaviour
{
    public List<GameObject> items;
    public Transform content;
    public ScrollRect scrollView;
    public GameObject[] transition;
    public GameObject[] itemPrefabList;

    [NonSerialized]
    public bool flag = false;               //このスクリプトを動かすフラグ
    [NonSerialized]
    public int number = 0;

    private float transitionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in content)
        {
            items.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ItemListUIManagerのflag = " + flag);
        var description = transition[1].GetComponent<DescriptionUIManager>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetComponent<ItemInformation>().ItemQuantity == 0)
            {
                var destroyObj = items[i];
                items.RemoveAt(i);
                Destroy(destroyObj);
            }
        }

        if (flag)
        {
            transitionTime += Time.deltaTime;

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) && number < items.Count - 1)
            {
                number++;
                scrollView.verticalNormalizedPosition -= 1f / (items.Count - 1f);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) && number > 0)
            {
                number--;
                scrollView.verticalNormalizedPosition += 1f / (items.Count - 1f);
            }

            EventSystem.current.SetSelectedGameObject(items[number]);

            description.descriptionText.text = items[number].GetComponent<ItemInformation>().Item.text;

            if (transitionTime > 0.2f)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    transition[0].GetComponent<EquipmentItemUIManager>().flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
                else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    description.flag = true;
                    this.flag = false;
                    transitionTime = 0f;
                }
            }
        }
        else if (!flag)
        {
            if (!description.flag && !description.choiceFlag)
            {
                description.descriptionText.text = null;
            }
        }
    }
    public void AddItem(GameObject newItem, int quantity, bool itemFlag)
    {
        if (!itemFlag)
        {
            GameObject addItem = Instantiate<GameObject>(newItem, content.transform);
            addItem.GetComponent<ItemInformation>().ItemQuantity += quantity;
            items.Add(addItem.gameObject);
        }
        else
        {
            foreach (var item in items)
            {
                if (item.GetComponent<ItemInformation>().Item.itemType == newItem.GetComponent<ItemInformation>().Item.itemType)
                {
                    item.GetComponent<ItemInformation>().ItemQuantity += quantity;
                }
            }
        }
    }
}
