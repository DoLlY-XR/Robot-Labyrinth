using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour
{
    public EquipmentParts equipmentParts;
    public PartsListUIManager partsContent;
    public ItemListUIManager itemContent;
    public List<GameObject> parts;
    public List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        parts = partsContent.parts;
        items = itemContent.items;
    }

    public void AddParts(GameObject newParts)
    {
        bool partsGet = true;

        Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")");

        if (equipmentParts.WeaponInfo.partsType.ToString() == newParts.GetComponent<PartsInformation>().Parts.partsType.ToString() &&
            equipmentParts.WeaponInfo.power == newParts.GetComponent<PartsInformation>().Parts.power)
        {
            partsGet = false;

            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に装備しているため獲得できませんでした。");
        }

        if (equipmentParts.ShieldInfo.partsType.ToString() == newParts.GetComponent<PartsInformation>().Parts.partsType.ToString() &&
            equipmentParts.ShieldInfo.power == newParts.GetComponent<PartsInformation>().Parts.power)
        {
            partsGet = false;

            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に装備しているため獲得できませんでした。");
        }

        foreach (GameObject childParts in parts)
        {
            if (childParts.GetComponent<PartsInformation>().Parts.partsType == newParts.GetComponent<PartsInformation>().Parts.partsType &&
                childParts.GetComponent<PartsInformation>().Parts.power == newParts.GetComponent<PartsInformation>().Parts.power)
            {
                partsGet = false;

                Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "をドロップしましたが、既に持っているため獲得できませんでした。");

                break;
            }
        }

        if (partsGet)
        {
            partsContent.AddParts(newParts);

            Debug.Log(newParts.GetComponent<PartsInformation>().Parts.partsType + "(" + newParts.GetComponent<PartsInformation>().Parts.power + ")" + "を獲得");
        }
    }

    public void AddItem(ItemInformation.ItemType itemType)
    {
        Debug.Log("itemType = " + itemType);
        foreach (GameObject item in items)
        {
            if (item.GetComponent<ItemInformation>().Item.itemType == itemType)
            {
                if (item.GetComponent<ItemInformation>().Item.maxQuantity > item.GetComponent<ItemInformation>().Item.quantity)
                {
                    itemContent.AddItem(itemType);

                    Debug.Log(itemType + "を獲得");
                }
                else
                {
                    Debug.Log(itemType + "をドロップしましたが、手持ちがいっぱいで獲得できませんでした。");
                }
            }
        }
    }
}
